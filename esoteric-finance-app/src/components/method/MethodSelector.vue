<template>
    <div class="post">
        <div v-if="post" class="content">
            <v-select class="selectMethod"
                      taggable
                      multiple
                      label="name"
                      :options="dropdown"
                      v-model="selectedMethods"
                      :placeholder="dropdownPlaceholder">
                <template v-slot:no-options="{ name }">
                    {{ name }}
                </template>
                <template v-slot:option="{ name }">
                    {{ name }}
                </template>
                <template v-slot:selected-option="{ name, amount}">
                    {{ name }}, {{ usd.format(amount) }}
                </template>
            </v-select>
            <span class="error-select">{{ dropdownErrors[0] }}</span>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { computed, ref, onMounted } from 'vue';
    import type { Ref } from 'vue';
    import { NamedAmountType } from '@/types/global-types';
    import { useStore } from '@/store';
    import emitter from '@/plugins/mitt';

    const store = useStore();

    /* ref */
    const loading: Ref<boolean> = ref(false);
    const post: Ref<null | NamedAmountType[]> = ref(null);
    const dropdownPlaceholder: Ref<string> = ref('Select method...');
    const dropdownErrors: Ref<string[]> = ref(['']);
    /* computed */
    const usd = computed<Intl.NumberFormat>(() => new Intl.NumberFormat("en-US", { style: "currency", "currency": "USD" }));
    const dropdown = computed<NamedAmountType[]>(() => post.value ?? []);
    const selectedMethods = computed<NamedAmountType[]>({
        get() {
            return store.state.selectedMethods;
        },
        set(value) {
            if (value && value.length > 0) {
                console.log('settings methods...', value);
                value.forEach((v, i, a) => {
                    if (!v.amount || v.amount == 0) {
                        let amount = prompt(`What was the transaction amount for ${v.name}?`, "0")
                        while (Number.isNaN(amount) && Number(amount) !== 0) {
                            amount = prompt(`What was the transaction amount for ${v.name}? (Please enter a number)`, "0")
                        }
                        a[i].amount = Number(amount);
                    }
                });
            }
            store.dispatch('setMethods', value);
        }
    });
    /* methods */
    function fetchData(): void {
        post.value = null;
        loading.value = true;

        fetch(`method`, {
            method: 'GET'
        })
            .then(r => {
                if (r.status == 404) {
                    return [];
                } else {
                    return r.json()
                }
            })
            .then(json => {
                post.value = json as NamedAmountType[];
                loading.value = false;
                return;
            });
    }

    emitter.on('savedTransaction', () => fetchData());

    onMounted(() => fetchData());
</script>