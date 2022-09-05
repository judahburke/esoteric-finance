import mitt, { Emitter } from 'mitt';
import { PaymentEvents } from '@/types/mitt.d';

const emitter: Emitter<PaymentEvents> = mitt<PaymentEvents>();

export default emitter;