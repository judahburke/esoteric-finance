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
                        <th class="">Initiator</th>
                        <th class="">Recipient</th>
                        <th class="">Methods</th>
                        <th class="">Description</th>
                        <th class="">Options</th>
                    </tr>
                </thead>
                <tbody class="is-selected">
                    <tr>
                        <td class="">
                            <v-datepicker v-model="selectedTransactionDate"></v-datepicker>
                        </td>
                        <td class="">
                            {{ sumMethods(selectedTransaction)}}
                        </td>
                        <td class="">
                            <NamedEntitySelector v-model="selectedInitiator" :fetchRoute="initiatorRoute" :dropdownPlaceholder="initiatorPlaceholder"></NamedEntitySelector>
                        </td>
                        <td class="">
                            <NamedEntitySelector v-model="selectedRecipient" :fetchRoute="recipientRoute" :dropdownPlaceholder="recipientPlaceholder"></NamedEntitySelector>
                        </td>
                        <td class="">
                            <MethodSelector></MethodSelector>
                        </td>
                        <td class="">
                            <DetailSelector></DetailSelector>
                        </td>
                        <td class="">
                            <input class="button" type="button" value="Save" @click="saveTransaction" />
                            <input class="button" type="button" value="Cancel" @click="setTransaction(null)" />
                        </td>
                    </tr>
                </tbody>
                <tbody>
                    <tr class="" v-for="transaction in tableData" :key="transaction.transactionId">
                        <td class=""><v-datepicker v-model="transaction.transactionDate" disabled></v-datepicker></td>
                        <td class="">{{ sumMethods(transaction)}}</td>
                        <td class="">{{ transaction.initiator?.name }}</td>
                        <td class="">{{ transaction.recipient?.name }}</td>
                        <td class="">{{ concatMethods(transaction) }}</td>
                        <td class="">{{ concatDetails(transaction) }}</td>
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
    import type { TransactionType } from '@/types/global-types';
    import NamedEntitySelector from '@/components/NamedEntitySelector.vue';
    import MethodSelector from '@/components/MethodSelector.vue';
    import DetailSelector from '@/components/DetailSelector.vue';
    import { useStore } from '@/store';

    const store = useStore();

    /* ref */
    const loading: Ref<boolean> = ref(false);
    const post: Ref<null | TransactionType[]> = ref(null);
    const initiatorPlaceholder: Ref<string> = ref('Select initiator...');
    const initiatorRoute: Ref<string> = ref('initiator');
    const recipientPlaceholder: Ref<string> = ref('Select recipient...');
    const recipientRoute: Ref<string> = ref('recipient');
    /* computed */
    const usd = computed<Intl.NumberFormat>(() => new Intl.NumberFormat("en-US", { style: "currency", "currency": "USD" }));
    const tableData = computed<TransactionType[]>(() => post.value?.filter(t => t.id !== store.state.selectedTransactionId) ?? []);
    //const selectedTransactionId = computed(() => store.state.selectedTransactionId);
    const selectedTransactionDate = computed({
        get() {
            return store.state.selectedTransactionDate;
        },
        set(value) {
            store.dispatch('setTransactionDate', value);
        }
    });
    //const selectedPostedDate = computed(() => store.state.selectedPostedDate);
    const selectedInitiator = computed({
        get() {
            return store.state.selectedInitiator;
        },
        set(value) {
            store.dispatch('setInitiator', value);
        }
    });
    const selectedRecipient = computed({
        get() {
            return store.state.selectedRecipient;
        },
        set(value) {
            store.dispatch('setRecipient', value);
        }
    });
    //const selectedMethods = computed(() => store.state.selectedMethods);
    //const selectedDetails = computed(() => store.state.selectedDetails);
    const selectedTransaction = computed<TransactionType>(() => {
        return {
            id: store.state.selectedTransactionId,
            transactionDate: store.state.selectedTransactionDate,
            postedDate: store.state.selectedPostedDate,
            initiator: store.state.selectedInitiator,
            recipient: store.state.selectedRecipient,
            methods: store.state.selectedMethods,
            details: store.state.selectedDetails,
        };
    });
    const isValid = computed<Boolean>(() => {
        if (true
            && selectedTransaction.value.transactionDate
            && selectedTransaction.value.initiator
            && selectedTransaction.value.initiator.name
            && selectedTransaction.value.recipient
            && selectedTransaction.value.recipient.name
            && selectedTransaction.value.methods
            && selectedTransaction.value.methods.length > 0
            && selectedTransaction.value.details
            && selectedTransaction.value.methods.length > 0) {
            return true;
        } else { return false; }
    });
    /* methods */
    function setTransaction(payload: TransactionType | null): void {
        store.dispatch('setTransactionId', payload?.id ?? 0);
        store.dispatch('setTransactionDate', payload?.transactionDate ?? new Date());
        store.dispatch('setPostedDate', payload?.postedDate ?? null);
        store.dispatch('setInitiator', { id: payload?.initiator?.id ?? 0, name: payload?.initiator?.name ?? null })
        store.dispatch('setRecipient', { id: payload?.recipient?.id ?? 0, name: payload?.recipient?.name ?? null });
        store.dispatch('setMethods', payload?.methods?.map(m => {
            return { id: m.id ?? 0, name: m.name ?? null, amount: m.amount };
        }) ?? []);
        store.dispatch('setDetails', payload?.details?.map(c => {
            return { id: c.id ?? 0, description: c.description, multiplier: c.multiplier, category: c.category };
        }) ?? []);
    }
    function saveTransaction(): void {
        if (isValid.value) {
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
                })
        }
        else {
            // todo set invalid states per selector
            console.log('nuh uh');
            alert('Invalid values, try again')
        }
    }
    function sumMethods(transaction: TransactionType): number {
        let sum = 0;
        transaction.methods?.forEach(m => sum += m.amount);
        return sum;
    }
    function concatMethods(transaction: TransactionType): string {
        let output = '';
        transaction.methods?.forEach(m => output = `${output}${m.name} (${usd.value.format(m.amount)});`)
        return output.substr(0, output.length - 1);
    }
    function concatDetails(transaction: TransactionType): string {
        let output = '';
        const sum = sumMethods(transaction);
        transaction.details?.forEach(c => output = `${output}${c.category?.name} - ${c.description} (${usd.value.format((c?.multiplier ?? 1) * sum)});`)
        return output.substr(0, output.length - 1);
    }
    function fetchData(): void {
        post.value = null;
        loading.value = true;

        fetch('transaction', {
            method: 'GET'
                })
                    .then(r => r.json())
            .then(json => {
                post.value = json as TransactionType[];
                loading.value = false;
                return;
            });
    }

    //watch('$route', 'fetchData');

    onMounted(() => fetchData());
</script>