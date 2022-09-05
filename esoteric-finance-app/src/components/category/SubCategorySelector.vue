<template>
    <div class="post">
        <div v-if="post" class="content">
            <v-select class="selectSubCategory"
                      taggable
                      multiple
                      push-tags
                      label="name"
                      :options="dropdown"
                      v-model="selectedSubCategories"
                      :placeholder="dropdownPlaceholder">
                <template v-slot:no-options="{ name }">
                    {{ name }}
                </template>
                <template v-slot:option="{ name }">
                    {{ name }}
                </template>
                <template v-slot:selected-option="{ name, multiplier}">
                    {{ name }}, {{ percentage.format(multiplier) }}
                </template>
            </v-select>
            <span class="error-select">{{ dropdownErrors[0] }}</span>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { computed, ref, onMounted, watch } from 'vue';
    import type { Ref } from 'vue';
    import { NamedMultiplierType } from '@/types/global-types';
    import { useStore } from '@/store';
    import emitter from '@/plugins/mitt';

    const store = useStore();

    /* refs */
    const loading: Ref<boolean> = ref(false);
    const post: Ref<null | NamedMultiplierType[]> = ref(null);
    const dropdownPlaceholder: Ref<string> = ref('Select sub-category...');
    const dropdownErrors: Ref<string[]> = ref(['']);
    /* computed */
    const percentage = computed<Intl.NumberFormat>(() => new Intl.NumberFormat("en-US", { style: "percent" }));
    const dropdown = computed<NamedMultiplierType[]>(() => post.value ?? []);
    const selectedCategoryId = computed(() => store.state.selectedCategory ? store.state.selectedCategory.id : 0);
    const selectedSubCategories = computed<NamedMultiplierType[]>({
        get() {
            return store.state.selectedSubCategories;
        },
        set(value) {
            if (value && value.length > 0) {
                value.forEach((v, i, a) => {
                    let multiplier = prompt(`What portion (0.01 to 1.00) of the purchase was to ${v.name}?`, "1")
                    while (Number.isNaN(multiplier) && Number(multiplier) > 0 && Number(multiplier) <= 1) {
                        multiplier = prompt(`What portion (0.01 to 1.00) of the purchase was to ${v.name}? (Please enter a number between 0.01 and 1.00)`, "1")
                    }
                    a[i].multiplier = Number(multiplier);
                })

                for (let i = 0; i < value.length && sum(value) != 1; i++) {
                    var adjustment = 1 - sum(value);
                    var curMultiplier = value[i].multiplier ?? 0;
                    var newMultiplier = Math.max(Math.min(curMultiplier + adjustment, 1), 0.01)
                    alert(`The purchase must be split evenly between all subcategories. ${value[0].name} will now be ${percentage.value.format(newMultiplier)}.`);
                    value[i].multiplier = newMultiplier;
                }
            }
            store.dispatch('setSubCategories', value);
        }
    })
    /* methods */
    //function createSubCategory(value: any): any {
    //    console.log('create-option', value);
    //    return value;
    //}
    function sum(subCategories: NamedMultiplierType[]) {
        let total = 0;
        subCategories.forEach((v: NamedMultiplierType) => total += v.multiplier ?? 0);
        return total;
    }
    function fetchData(): void {
        post.value = null;
        loading.value = true;

        if (selectedCategoryId.value > 0) {
            fetch(`category/${selectedCategoryId.value}/subcategory`, {
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
                    post.value = json as NamedMultiplierType[];
                    loading.value = false;
                    return;
                });
        }
    }

    watch(selectedCategoryId, () => fetchData());

    emitter.on('savedTransaction', () => fetchData());

    onMounted(() => fetchData());
</script>