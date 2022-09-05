<template>
    <div class="post">
        <div v-if="post" class="content">
            <v-select class="selectRecipient"
                      taggable
                      push-tags
                      label="name"
                      :options="dropdown"
                      v-model="selectedRecipient"
                      :placeholder="dropdownPlaceholder"/>
            <span class="error-select">{{ dropdownErrors[0] }}</span>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent, computed } from 'vue';
    import { NamedType } from '@/types/global-types';
    import { useStore } from '@/store';
    import emitter from '@/plugins/mitt';

    interface Data {
        loading: boolean,
        post: null | NamedType[],
        dropdown: NamedType[],
        dropdownPlaceholder: string,
        dropdownErrors: string[]
    }

    export default defineComponent({
        model: {
            prop: 'value',
            event: 'input'
        },
        data(): Data {
            return {
                loading: false,
                post: null,
                get dropdown() {
                    return this.post ?? [];
                },
                dropdownPlaceholder: 'Select method...',
                dropdownErrors: ['']
            };
        },
        created() {
            emitter.on('savedTransaction', () => this.fetchData());
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        setup() {
            const store = useStore();

            return {
                selectedRecipient: computed<NamedType | null>({
                    get() {
                        return store.state.selectedRecipient;
                    },
                    set(value) {
                        store.dispatch('setRecipient', value);
                    }
                })
            }
        },
        watch: {
            // call again the recipient if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData(): void {
                this.post = null;
                this.loading = true;

                fetch('recipient', {
                    method: 'GET'
                })
                    .then(r => r.json())
                    .then(json => {
                        this.post = json as NamedType[];
                        this.loading = false;
                        return;
                    });
            }
        },
    });
</script>