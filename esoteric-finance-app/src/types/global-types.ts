type NamedType = {
    id: null | number,
    name: null | string,
}
type MethodType = NamedType & {
    amount: number
}
type DetailType = {
    id: null | number,
    description: null | string,
    multiplier: null | number,
    category: null | CategoryType
}
type CategoryType = NamedType & {
    details: null | DetailType[]
}

type TransactionType = {
    id: null | number,
    transactionDate: Date,
    postedDate: null | Date,
    initiator: null | NamedType,
    methods: null | MethodType[],
    recipient: null | NamedType,
    details: null | DetailType[]
}

export {
    NamedType,
    MethodType,
    CategoryType,
    DetailType,
    TransactionType
}