<template>
  <table class="table table-striped table-hover caption-top">
    <caption>{{ caption }}</caption>
    <tbody>
      <tr>
        <th>Ticker / Target Prices</th>
        <th>Current Price / Recomendations</th>
      </tr>
      <tr v-for="(item, index) in stocks" :key="index">
        <td>
          <small class="text-muted">{{ item['ticker'] }}</small>
          <div>{{ item['CompanyName'] }}</div>
          <div><small class="text-muted">{{ item['industry'] }}</small></div>
          <small class="text-muted">{{ item['targetPrices'] }}</small>
        </td>
        <td class="relative-cell">
          <div class="d-flex justify-content-between align-items-center mb-3">
            <span>{{ item['currentPrice'] }}</span>
            <div class="d-flex justify-content-between align-items-center">
              <span class="text-muted me-2">{{ item['mychoice'] }}</span>
              <span class="badge rounded-pill bg-success">{{ item['mychoicePercent'] }}</span>
            </div>
          </div>
          <small class="text-muted aligned-text">{{ item['rs'] }}({{ item['rt'] }})</small>
        </td>
      </tr>
    </tbody>
  </table>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { IRowViewModel } from '@/views/PageReport/PageReport.types';

export default defineComponent({
  props: {
    caption: String,
    stocks: Array as PropType<IRowViewModel[]>,
  },
});
</script>

<style lang="scss" scoped>
.relative-cell {
  position: relative;
}

.aligned-text {
  position: absolute;
  bottom: 0.5rem;
  right: 0.5rem;
}
</style>
