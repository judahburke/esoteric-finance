import type { ComponentCustomProperties } from 'vue'
import { Store } from 'vuex'
import type { NamedType, DetailType, MethodType } from '@/types/global-types';

export interface State {
    selectedTransactionId: number,
    selectedTransactionDate: Date,
    selectedPostedDate: null | Date,
    selectedAmount: number,
    selectedDetails: DetailType[],
    selectedInitiator: null | NamedType,
    selectedMethods: MethodType[],
    selectedRecipient: null | NamedType,
}

declare module '@vue/runtime-core' {
    // provide typings for `this.$store`
    interface ComponentCustomProperties {
        $store: Store<State>
    }
}