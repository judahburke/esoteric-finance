import type { NamedType, CategoryType, DetailType, MethodType } from '@/types/global-types';
import type { State } from '@/types/vuex.d';
import { InjectionKey } from 'vue'
import { createStore, useStore as baseUseStore, Store } from 'vuex'

export const key: InjectionKey<Store<State>> = Symbol()

export const store = createStore<State>({
    state: {
        selectedTransactionId: 0,
        selectedTransactionDate: new Date(),
        selectedPostedDate: null,
        selectedAmount: 0,
        selectedDetails: [],
        selectedInitiator: null,
        selectedMethods: [],
        selectedRecipient: null,
    },

    getters: {
        selectedTransactionId(state) {
            return state.selectedTransactionId;
        },
        selectedTransactionDate(state) {
            return state.selectedTransactionDate;
        },
        selectedPostedDate(state) {
            return state.selectedPostedDate;
        },
        selectedAmount(state) {
            return state.selectedAmount;
        },
        selectedDetails(state) {
            return state.selectedDetails;
        },
        selectedInitiator(state) {
            return state.selectedInitiator;
        },
        selectedMethod(state) {
            return state.selectedMethods;
        },
        selectedRecipient(state) {
            return state.selectedRecipient;
        }
    },

    mutations: {
        SET_TRANSACTION_ID(state: State, payload: number) {
            state.selectedTransactionId = payload;
        },
        SET_TRANSACTION_DATE(state: State, payload: Date) {
            state.selectedTransactionDate = payload;
        },
        SET_POSTED_DATE(state: State, payload: null | Date) {
            state.selectedPostedDate = payload
        },
        SET_AMOUNT(state: State, payload: number) {
            state.selectedAmount = payload;
        },
        SET_DETAILS(state: State, payload: DetailType[]) {
            state.selectedDetails = payload;
        },
        PUSH_DETAIL(state: State, payload: DetailType) {
            state.selectedDetails.push(payload);
        },
        REMOVE_DETAIL(state: State, payload: DetailType) {
            const i = state.selectedDetails.indexOf(payload);
            if (i > -1) {
                state.selectedDetails = state.selectedDetails.splice(i, 1);
            }
        },
        SET_INITIATOR(state: State, payload: NamedType) {
            state.selectedInitiator = payload;
        },
        SET_METHODS(state: State, payload: MethodType[]) {
            state.selectedMethods = payload;
        },
        SET_RECIPIENT(state: State, payload: NamedType) {
            state.selectedRecipient = payload;
        },
    },

    actions: {
        setTransactionId({ commit }, payload: number) {
            commit("SET_TRANSACTION_ID", payload);
        },
        setTransactionDate({ commit }, payload: Date) {
            commit("SET_TRANSACTION_DATE", payload);
        },
        setPostedDate({ commit }, payload: null | Date) {
            commit("SET_POSTED_DATE", payload);
        },
        setAmount({ commit }, payload: number) {
            commit("SET_AMOUNT", payload);
        },
        setDetails({ commit }, payload: DetailType[]) {
            commit("SET_DETAILS", payload);
        },
        setInitiator({ commit }, payload: NamedType) {
            commit("SET_INITIATOR", payload);
        },
        setMethods({ commit }, payload: MethodType[]) {
            commit("SET_METHODS", payload);
        },
        setRecipient({ commit }, payload: NamedType) {
            commit("SET_RECIPIENT", payload);
        },
    }
})

export function useStore() {
    return baseUseStore(key)
}