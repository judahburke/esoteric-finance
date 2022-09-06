<template>
    <div class="post">
        <div v-if="post" class="content">
            <v-select class="selectDetail"
                      taggable
                      multiple
                      push-tags
                      label="description"
                      :options="dropdown"
                      v-model="selectedDetails"
                      :placeholder="dropdownPlaceholder">
                <template v-slot:option="{ description, category }">
                    {{ category?.name }} - {{ description }}
                </template>
                <template v-slot:selected-option="{ description, category, multiplier}">
                    {{ category?.name }} - {{ description }}, {{ percentage.format(multiplier) }}
                </template>
            </v-select>
            <span class="error-select">{{ dropdownErrors[0] }}</span>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { computed, ref, onMounted, watch } from 'vue';
    import type { Ref } from 'vue';
    import { DetailType, CategoryType } from '@/types/global-types';
    import { useStore } from '@/store';

    const store = useStore();

    /* refs */
    const loading: Ref<boolean> = ref(false);
    const post: Ref<null | CategoryType[]> = ref(null);
    const dropdownPlaceholder: Ref<string> = ref('Select details...');
    const dropdownErrors: Ref<string[]> = ref(['']);
    /* computed */
    const percentage = computed<Intl.NumberFormat>(() => new Intl.NumberFormat("en-US", { style: "percent" }));
    const dropdown = computed<DetailType[]>(() => post.value?.flatMap(c => c.details?.map(d => {
        return {
            id: d.id,
            description: d.description,
            multiplier: d.multiplier,
            category: {
                id: c.id,
                name: c.name
            } as CategoryType
        } as DetailType;
    }) ?? []) ?? []);
    const selectedTransactionId = computed(() => store.state.selectedTransactionId);
    const selectedDetails = computed<DetailType[]>({
        get() {
            return store.state.selectedDetails ?? [];
        },
        set(value) {
            if (value && value.length > 0) {
                value.forEach((v, i, a) => {
                    if (!v.category) {
                        let newCategory: CategoryType = {
                            id: 0,
                            name: prompt('What is the category of this purchase?', 'Miscellaneous'),
                            details: []
                        };
                        let found = false;
                        dropdown.value.forEach(d => {
                            if (!found && d.category && d.category.name?.toLowerCase() === newCategory.name?.toLowerCase()) {
                                newCategory = d.category;
                                found = true;
                            }
                        });
                        v.category = newCategory;
                    }
                    let multiplier = prompt(`What percentage (1 to 100) of the purchase was to ${v.description}?`, "1")
                    while (Number.isNaN(multiplier) && Number(multiplier) > 0 && Number(multiplier) <= 100) {
                        multiplier = prompt(`What percentage (1 to 100) of the purchase was to ${v.description}? (Please enter a number between 1 and 100)`, "1")
                    }
                    a[i].multiplier = Number(multiplier) / 100;
                })

                for (let i = 0; i < value.length && sum(value) != 1; i++) {
                    var adjustment = 1 - sum(value);
                    var curMultiplier = value[i].multiplier ?? 0;
                    var newMultiplier = Math.max(Math.min(curMultiplier + adjustment, 1), 0.01)
                    alert(`The purchase must be split evenly between all categories. ${value[0].description} will now be ${percentage.value.format(newMultiplier)}.`);
                    value[i].multiplier = newMultiplier;
                }
            }
            store.dispatch('setDetails', value);
        }
    })
    /* methods */
    function sum(details: DetailType[]) {
        let total = 0;
        details.forEach((v: DetailType) => total += v.multiplier ?? 0);
        return total;
    }
    function fetchData(): void {
        post.value = null;
        loading.value = true;

        fetch(`category`, {
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
                post.value = json as CategoryType[];
                loading.value = false;
                return;
            });
    }
    /* watch */
    watch(selectedTransactionId, (newId) => {
        if (newId === 0) { fetchData(); }
    });

    onMounted(() => fetchData());
</script>