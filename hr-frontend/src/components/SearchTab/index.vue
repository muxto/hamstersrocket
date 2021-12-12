<template>
  <div class="form-floating mb-3">
    <input
      type="text"
      class="form-control"
      id="searchInput"
      placeholder="Тикер компании"
      v-model="searchValue"
    >
    <label for="searchInput">Тикер компании</label>
  </div>

  <search-results
    v-if="searchTable.length"
    caption="Результаты поиска"
    :stocks="searchTable"
  />

  <div v-else class="alert alert-danger" role="alert">
    Результаты не найдены.
  </div>
</template>

<script lang="ts">
import {
  defineComponent,
  ref,
  PropType,
  computed,
} from 'vue';
import SearchResults from './SearchResults.vue';
import { IRowViewModel } from '@/views/PageReport/PageReport.types';

export default defineComponent({
  name: 'SearchTab',
  components: { SearchResults },
  props: {
    tickersData: {
      type: Array as PropType<IRowViewModel[]>,
      required: true,
    },
  },
  setup(props) {
    const searchValue = ref('');

    const searchTable = computed(() => props.tickersData
      .filter((result) => result.ticker.toLowerCase().startsWith(searchValue.value.toLowerCase())));

    return {
      searchValue,
      searchTable,
    };
  },
});
</script>
