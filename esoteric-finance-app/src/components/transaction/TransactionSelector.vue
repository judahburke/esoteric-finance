<template>
    <div class="post">
        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="post" class="content">
            <table class="table">
                <thead class="">
                    <tr>
                        <th class="">Date</th>
                        <th class="">Amount</th>
                        <th class="">Recipient</th>
                        <th class="">Categories</th>
                        <th class="">Methods</th>
                        <th class="">Options</th>
                    </tr>
                </thead>
                <tbody class="is-selected">
                    <tr>
                        <td class="">
                            <v-datepicker v-model="selectedTransactionDate"></v-datepicker>
                        </td>
                        <td class="">
                            {{ sumAmount(selectedTransaction)}}
                        </td>
                        <td class="">
                            <RecipientSelector></RecipientSelector>
                        </td>
                        <td class="">
                            <div class="columns is-gapless">
                                <CategorySelector class="column"></CategorySelector>
                                <SubCategorySelector class="column"></SubCategorySelector>
                            </div>
                        </td>
                        <td class="">
                            <MethodSelector></MethodSelector>
                        </td>
                        <td class="">
                            <input class="button" type="button" value="Save" @click="saveTransaction" />
                        </td>
                    </tr>
                </tbody>
                <tbody>
                    <tr class="" v-for="transaction in tableData" :key="transaction.transactionId">
                        <td class=""><v-datepicker v-model="transaction.transactionDate" disabled></v-datepicker></td>
                        <td class="">{{ sumAmount(transaction)}}</td>
                        <td class="">{{ transaction.recipient.recipient }}</td>
                        <td class="">{{ concatCategories(transaction) }}</td>
                        <td class="">{{ concatMethods(transaction) }}</td>
                        <td class=""><input class="button" type="button" value="Edit" @click="setTransaction(transaction)" /></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { computed, ref, onMounted } from 'vue';
    import { Ref } from 'vue';
    import type { TransactionType, TransactionsType } from '@/types/global-types';
    import { TransactionRecipient, TransactionMethod, TransactionCategory } from '@/types/global-types';
    import { useStore } from '@/store';
    import RecipientSelector from '@/components/recipient/RecipientSelector.vue';
    import CategorySelector from '@/components/category/CategorySelector.vue';
    import SubCategorySelector from '@/components/category/SubCategorySelector.vue';
    import MethodSelector from '@/components/method/MethodSelector.vue';
    import emitter from '@/plugins/mitt';

    const store = useStore();

    /* ref */
    const loading: Ref<boolean> = ref(false);
    const post: Ref<null | TransactionsType> = ref(null);
    /* computed */
    const usd = computed<Intl.NumberFormat>(() => new Intl.NumberFormat("en-US", { style: "currency", "currency": "USD" }));
    const tableData = computed<TransactionType[]>(() => post.value?.filter(t => t.transactionId !== selectedTransactionId.value) ?? []);
    const selectedTransactionId = computed(() => store.state.selectedTransactionId);
    const selectedTransactionDate = computed({
        get() {
            return store.state.selectedTransactionDate;
        },
        set(value) {
            store.dispatch('setTransactionDate', value);
        }
    });
    //const selectedPostedDate = computed(() => store.state.selectedPostedDate);
    const selectedCategory = computed(() => store.state.selectedCategory);
    const selectedSubCategories = computed(() => store.state.selectedSubCategories);
    const selectedRecipient = computed(() => store.state.selectedRecipient);
    const selectedMethods = computed(() => store.state.selectedMethods);
    const selectedTransaction = computed<TransactionType>(() => {
        return {
            transactionId: store.state.selectedTransactionId,
            transactionDate: store.state.selectedTransactionDate,
            postedDate: store.state.selectedPostedDate,
            recipient: new TransactionRecipient(store.state.selectedRecipient),
            methods: store.state.selectedMethods.map((m) => new TransactionMethod(m)),
            categories: store.state.selectedSubCategories.map((sc) => new TransactionCategory(store.state.selectedCategory, sc))
        };
    });
    const isValidTransactionDate = computed<Boolean>(() => selectedTransaction.value && selectedTransaction.value.transactionDate && true);
    const isValidCategory = computed<Boolean>(() => true && selectedCategory.value != null && selectedCategory.value.name != null);
    const isValidSubCategories = computed<Boolean>(() => selectedSubCategories.value && selectedSubCategories.value.length > 0);
    const isValidRecipient = computed<Boolean>(() => true && selectedRecipient.value != null && selectedRecipient.value.name != null);
    const isValidMethods = computed<Boolean>(() => selectedMethods.value && selectedMethods.value.length > 0);
    /* methods */
    function setTransaction(payload: TransactionType | null): void {
        store.dispatch('setTransactionId', payload?.transactionId ?? 0);
        store.dispatch('setTransactionDate', payload?.transactionDate ?? new Date());
        store.dispatch('setPostedDate', payload?.postedDate ?? null);
        store.dispatch('setRecipient', { id: payload?.recipient?.recipientId ?? 0, name: payload?.recipient?.recipient ?? null });
        store.dispatch('setMethods', payload?.methods?.map(m => {
            return { id: m.methodId, name: m.method, amount: m.amount };
        }) ?? []);
        const category = payload && payload.categories && payload.categories.length > 0
            ? payload.categories[0] : null;
        store.dispatch('setCategory', { id: category?.categoryId ?? 0, name: category?.category ?? null });
        store.dispatch('setSubCategories', payload?.categories?.map(c => {
            return { id: c.subCategoryId, name: c.subCategory, multiplier: c.multiplier };
        }) ?? []);
        emitter.emit('changedTransaction', null);
    }
    function saveTransaction(): void {
        if (isValidTransactionDate.value
            && isValidCategory.value
            && isValidSubCategories.value
            && isValidRecipient.value
            && isValidMethods.value) {
            fetch('transaction', {
                method: 'PUT',
                body: JSON.stringify(selectedTransaction.value),
                headers: {
                    'content-type': 'application/json; charset=UTF-8'
                }
            })
                .then(() => {
                    setTransaction(null)
                    fetchData();
                    emitter.emit('savedTransaction');
                })
        }
        else {
            // todo set invalid states per selector
            alert('Invalid values, try again')
        }
    }
    function sumAmount(transaction: TransactionType): number {
        if (transaction && transaction.methods) {
            let output: number = 0;
            transaction.methods.forEach(m => {
                if (m.amount) {
                    output = output + m.amount
                }
            });
            return output;
        } else {
            return 0;
        }
    }
    function concatCategories(transaction: TransactionType): string {
        if (transaction && transaction.categories && transaction.categories.length > 0) {
            const sum = sumAmount(transaction);
            let output: string = '';
            transaction.categories.forEach(c => {
                if (c.category && c.subCategory && c.multiplier) {
                    output = `${output}${c.subCategory}, ${usd.value.format(c.multiplier * sum)};`;
                }
            });
            return `${transaction.categories[0].category} (${output.substring(0, output.length - 1)})`;
        } else {
            return '(none)'; //todo throw error
        }
    }
    function concatMethods(transaction: TransactionType): string {
        if (transaction && transaction.methods) {
            let output: string = '';
            transaction.methods.forEach(m => {
                if (m.method && m.amount) {
                    output = `${output}${m.method}, ${usd.value.format(m.amount)};`;
                }
            });
            return output.substring(0, output.length - 1);
        } else {
            return '(none)'; //todo throw error
        }
    }
    function fetchData(): void {
        post.value = null;
        loading.value = true;

        fetch('transaction', {
            method: 'GET'
                })
                    .then(r => r.json())
            .then(json => {
                post.value = json as TransactionsType;
                loading.value = false;
                return;
            });
    }

    //watch('$route', 'fetchData');

    onMounted(() => fetchData());
</script>