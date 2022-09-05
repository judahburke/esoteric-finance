type NamedType = {
    id: null | number,
    name: null | string,
}
type NamedAmountType = NamedType & {
    amount: null | number
}
type NamedMultiplierType = NamedType & {
    multiplier: null | number
}

type TransactionRecipientType = {
    recipientId: null | number,
    recipient: null | string,
}
class TransactionRecipient implements TransactionRecipientType {
    constructor(recipient: NamedType | null) {
        this.recipientId = recipient ? recipient.id : null;
        this.recipient = recipient ? recipient.name : null;
    }
    recipientId: null | number = null;
    recipient: null | string = null;
}

type TransactionMethodType = {
    methodId: null | number,
    method: null | string,
    amount: null | number,
}
type TransactionMethodsType = TransactionMethodType[];
class TransactionMethod implements TransactionMethodType {
    constructor(method: NamedAmountType | null) {
        this.methodId = method ? method.id : null;
        this.method = method ? method.name : null;
        this.amount = method ? method.amount : null;
    }
    methodId: null | number = null;
    method: null | string = null;
    amount: null | number = null;
}

type TransactionCategoryType = {
    categoryId: null | number,
    category: null | string,
    subCategoryId: null | number,
    subCategory: null | string,
    multiplier: null | number,
}
type TransactionCategoriesType = TransactionCategoryType[];
class TransactionCategory implements TransactionCategoryType {
    constructor(category: NamedType | null, subCategory: NamedMultiplierType | null) {
        this.categoryId = category ? category.id : null;
        this.category = category ? category.name : null;
        this.subCategoryId = subCategory ? subCategory.id : null;
        this.subCategory = subCategory ? subCategory.name : null;
        this.multiplier = subCategory ? subCategory.multiplier : null;
    }
    categoryId: number | null;
    category: string | null;
    subCategoryId: number | null;
    subCategory: string | null;
    multiplier: number | null;
}

type TransactionType = {
    transactionId: null | number,
    transactionDate: Date,
    postedDate: null | Date,
    recipient: null | TransactionRecipientType,
    methods: null | TransactionMethodsType,
    categories: null | TransactionCategoriesType
}
type TransactionsType = TransactionType[];
class Transaction implements TransactionType {
    private _recipient: null | TransactionRecipientType = null;
    private _methods: null | TransactionMethodsType = null;
    private _categories: null | TransactionCategoriesType = null;

    public transactionId: null | number = null;
    public transactionDate: Date = new Date();
    public postedDate: null | Date = null;
    public get recipient(): null | TransactionRecipientType {
        return this._recipient;
    }
    public set recipient(value: any) {
        this._recipient = value as TransactionRecipientType;
    }
    public get methods(): null | TransactionMethodsType {
        return this._methods;
    }
    public set methods(value: any) {
        this._methods = value as TransactionMethodsType
    }
    public get categories(): null | TransactionCategoriesType {
        return this._categories;
    }
    public set categories(value: any) {
        this._categories = value as TransactionCategoriesType
    }
}

export {
    NamedType,
    NamedAmountType,
    NamedMultiplierType,
    TransactionRecipientType,
    TransactionRecipient,
    TransactionMethodType,
    TransactionMethodsType,
    TransactionMethod,
    TransactionCategoryType,
    TransactionCategoriesType,
    TransactionCategory,
    TransactionType,
    TransactionsType,
    Transaction
}