import type { ComponentCustomProperties } from 'vue'
import { Store } from 'vuex'
import type { NamedType, NamedAmountType, NamedMultiplierType } from '@/types/global-types';

export interface State {
    selectedTransactionId: number,
    selectedCategory: null | NamedType,
    selectedSubCategories: NamedMultiplierType[],
    selectedTransactionDate: Date,
    selectedPostedDate: null | Date,
    selectedMethods: NamedAmountType[],
    selectedRecipient: null | NamedType,
}

declare module '@vue/runtime-core' {
    // provide typings for `this.$store`
    interface ComponentCustomProperties {
        $store: Store<State>
    }
}