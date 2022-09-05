import type { TransactionType } from '@/types/global-types';

export type PaymentEvents = {
    changedTransaction: null | TransactionType
    savedTransaction: void;
};

//declare module '@vue/runtime-core' {
//    export interface ComponentCustomProperties {
//        emitter: 
//    }
//}