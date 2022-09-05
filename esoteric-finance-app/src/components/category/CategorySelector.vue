<template>
    <div class="post">
        <div v-if="post" class="content">
            <v-select class="selectCategory"
                      taggable
                      push-tags
                      label="name"
                      :options="dropdown"
                      v-model="selectedCategory"
                      :placeholder="dropdownPlaceholder" />
            <span class="error-select">{{ dropdownErrors[0] }}</span>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { computed, onMounted, ref } from 'vue';
    import type { Ref } from 'vue';
    import type { NamedType } from '@/types/global-types';
    import { useStore } from '@/store';
    import emitter from '@/plugins/mitt';

    const store = useStore();

    /* ref */
    const loading: Ref<boolean> = ref(false);
    const post: Ref<null | NamedType[]> = ref(null);
    const dropdown = computed<NamedType[]>(() => post.value ?? []);
    const dropdownPlaceholder: Ref<string> = ref('Select category...');
    const dropdownErrors: Ref<string[]> = ref(['']);
    /* computed */
    const selectedCategory = computed<null | NamedType>({
        get() {
            return store.state.selectedCategory;
        },
        set(value) {
            store.dispatch('setCategory', value);
        }
    });
    /* methods */
    function fetchData(): void {
        post.value = null;
        loading.value = true;

        fetch('category', {
            method: 'GET'
        })
            .then(r => r.json())
            .then(json => {
                post.value = json as NamedType[];
                loading.value = false;
                return;
            });
    }

    emitter.on('savedTransaction', () => fetchData());

    onMounted(() => fetchData());
</script>