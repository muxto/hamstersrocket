<template>
  <div class="container">
    <h1 align="center">Hamster's Rocket!</h1>
    <p class="text-center mb-5">
      Сервис помогает иногда найти недооценные компании,
      показывая соотношение Прогнозы аналитиков / текущая цена.</p>

    <p class="mb-3">Дата последнего обновления: {{ updatedate }}</p>

    <ul class="nav nav-tabs mb-3">
      <li
        class="nav-item"
        v-for="tab in tabs"
        @click="switchTab(tab.value)"
        :key="tab.value"
      >
        <a :class="[
          'nav-link',
          tab.value === currentActiveTab && 'active'
        ]" href="#">{{ tab.label }}</a>
      </li>
    </ul>

    <small-table
      v-if="currentActiveTab === 'strongBuy'"
      caption="Лучшие акции по количеству рейтингов Strong Buy от аналитиков"
      :stocks="strongBuy"
    />

    <best-table
      v-if="currentActiveTab === 'mychoicePercentRecs'"
      caption="Самые недооценённые акции по версии Hamster's Rocket"
      :stocks="mychoicePercentRecs"
    />

    <search-tab
      v-if="currentActiveTab === 'search'"
      :tickersData="viewModel"
    />
  </div>
</template>

<script lang="ts">
import {
  defineComponent,
  onMounted,
  computed,
  ref,
} from 'vue';
import getReport from '../../api';
import SearchTab from '@/components/SearchTab/index.vue';
import SmallTable from '@/components/SmallTable.vue';
import BestTable from '@/components/BestTable.vue';
import { IReport } from './PageReport.types';
import { computeRowModel, computeRowViewModel, prepareTableData } from './pageReport.helpers';

export default defineComponent({
  name: 'PageReport',
  components: { SearchTab, SmallTable, BestTable },
  setup() {
    const reportData = ref<null | IReport>(null);

    onMounted(async () => {
      reportData.value = await getReport('report.json');
    });

    const updatedate = computed(() => {
      if (!reportData.value) return '';

      return new Date(reportData.value.UpdateDate).toLocaleString();
    });

    const model = computed(() => computeRowModel(reportData.value));
    const viewModel = computed(() => computeRowViewModel(model.value));

    const strongBuy = computed(() => prepareTableData(viewModel.value, 'strongBuy'));
    const mychoicePercentRecs = computed(() => prepareTableData(viewModel.value, 'mychoicePercent'));

    const tabs = [
      {
        label: '#1',
        value: 'mychoicePercentRecs',
      },
      {
        label: '#2',
        value: 'strongBuy',
      },
      {
        label: 'Поиск по тикеру',
        value: 'search',
      },
    ];

    const currentActiveTab = ref('mychoicePercentRecs');

    const switchTab = (newActiveTab: string) => {
      currentActiveTab.value = newActiveTab;
    };

    return {
      updatedate,
      viewModel,
      tabs,
      currentActiveTab,
      switchTab,
      strongBuy,
      mychoicePercentRecs,
    };
  },
});
</script>
