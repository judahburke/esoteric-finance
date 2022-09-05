import type { NamedType, NamedAmountType, NamedMultiplierType } from '@/types/global-types';
import type { State } from '@/types/vuex.d';
import { InjectionKey } from 'vue'
import { createStore, useStore as baseUseStore, Store } from 'vuex'

export const key: InjectionKey<Store<State>> = Symbol()

export const store = createStore<State>({
    state: {
        selectedTransactionId: 0,
        selectedTransactionDate: new Date(),
        selectedPostedDate: null,
        selectedCategory: null,
        selectedSubCategories: [],
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
        selectedCategory(state) {
            return state.selectedCategory;
        },
        selectedSubCategories(state) {
            return state.selectedSubCategories;
        },
        selectedMethods(state) {
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
        SET_CATEGORY(state: State, payload: NamedType) {
            state.selectedCategory = payload;
        },
        SET_SUBCATEGORIES(state: State, payload: NamedMultiplierType[]) {
            state.selectedSubCategories = payload;
        },
        PUSH_SUBCATEGORY(state: State, payload: NamedMultiplierType) {
            state.selectedSubCategories.push(payload);
        },
        CLEAR_SUBCATEGORIES(state: State) {
            state.selectedSubCategories = [];
        },
        REMOVE_SUBCATEGORY(state: State, payload: NamedMultiplierType) {
            const i = state.selectedSubCategories.indexOf(payload);
            if (i > -1) {
                state.selectedSubCategories = state.selectedSubCategories.splice(i, 1);
            }
        },
        SET_RECIPIENT(state: State, payload: NamedType) {
            state.selectedRecipient = payload;
        },
        SET_METHODS(state: State, payload: NamedAmountType[]) {
            state.selectedMethods = payload;
        },
        PUSH_METHOD(state: State, payload: NamedAmountType) {
            state.selectedMethods.push(payload);
        },
        CLEAR_METHODS(state: State) {
            state.selectedMethods = [];
        },
        REMOVE_METHOD(state: State, payload: NamedAmountType) {
            const i = state.selectedMethods.indexOf(payload);
            if (i > -1) {
                state.selectedMethods = state.selectedMethods.splice(i, 1);
            }
        },
    },

    actions: {
        setTransactionId({ commit }, id: number) {
            commit("SET_TRANSACTION_ID", id);
        },
        setTransactionDate({ commit }, date: Date) {
            commit("SET_TRANSACTION_DATE", date);
        },
        setPostedDate({ commit }, date: null | Date) {
            commit("SET_POSTED_DATE", date);
        },
        setCategory({ commit }, payload: NamedType) {
            commit("SET_CATEGORY", payload);
        },
        setSubCategories({ commit }, payload: NamedMultiplierType[]) {
            commit("SET_SUBCATEGORIES", payload);
        },
        pushSubCategory({ commit }, payload: NamedMultiplierType) {
            commit("PUSH_SUBCATEGORY", payload);
        },
        clearSubCategory({ commit }) {
            commit("CLEAR_SUBCATEGORY");
        },
        removeSubCategory({ commit }, payload: NamedMultiplierType) {
            commit("REMOVE_SUBCATEGORY", payload);
        },
        setRecipient({ commit }, payload: NamedType) {
            commit("SET_RECIPIENT", payload);
        },
        setMethods({ commit }, payload: NamedAmountType[]) {
            commit("SET_METHODS", payload);
        },
        pushMethod({ commit }, payload: NamedAmountType) {
            commit("PUSH_METHOD", payload);
        },
        clearMethods({ commit }) {
            commit("CLEAR_METHODS");
        },
        removeMethod({ commit }, payload: NamedAmountType) {
            commit("REMOVE_METHOD", payload);
        }
    }
})

export function useStore() {
    return baseUseStore(key)
}