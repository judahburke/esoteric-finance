<template>
    <div class="post">
        <div v-if="post" class="content">
            <v-select class="selectCategory"
                      taggable
                      push-tags
                      label="name"
                      :options="dropdown"
                      v-model="dropdownValue"
                      :placeholder="dropdownPlaceholder" />
            <span class="error-select">{{ dropdownErrors[0] }}</span>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { defineProps, defineEmits, computed, watch, onMounted, ref } from 'vue';
    import type { Ref } from 'vue';
    import type { NamedType } from '@/types/global-types';
    import { useStore } from '@/store';

    const store = useStore();

    /* emit */
    const emit = defineEmits(['update:modelValue']);
    /* props */
    const props = defineProps({
        modelValue: null,
        dropdownPlaceholder: String,
        fetchRoute: String
    });
    /* ref */
    const loading: Ref<boolean> = ref(false);
    const post: Ref<null | NamedType[]> = ref(null);
    const dropdown = computed<NamedType[]>(() => post.value ?? []);
    const dropdownErrors: Ref<string[]> = ref(['']);
    /* computed */
    const selectedTransactionId = computed(() => store.state.selectedTransactionId);
    const dropdownValue = computed({
        get() {
            return props.modelValue;
        },
        set(value) {
            emit('update:modelValue', value);
        }
    })
    /* methods */
    function fetchData(): void {
        post.value = null;
        loading.value = true;

        if (props.fetchRoute) {
            fetch(props.fetchRoute, {
                method: 'GET'
            })
                .then(r => r.json())
                .then(json => {
                    post.value = json as NamedType[];
                    loading.value = false;
                    return;
                });
        }
    }
    /* watch */
    watch(selectedTransactionId, (newId) => {
        if (newId === 0) { fetchData(); }
    });

    onMounted(() => {
        fetchData()

    });
</script>