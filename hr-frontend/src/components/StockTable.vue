<template>
  <table class="table table-striped table-hover caption-top">
    <caption>{{ caption }}</caption>
    <tbody>
      <tr>
        <th>#</th>
        <th>Logo</th>
        <th>Ticker</th>
        <th>Industry</th>
        <th>Current price</th>
        <th>Recommendations</th>
        <th>My Take Profit</th>
      </tr>
      <tr v-for="(item, index) in stocks" :key="index">
        <td>{{ index + 1 }}</td>
        <td>
          <img v-if="item['logo']" class="table-logo" width="32" :src="item['logo']" />
          <img v-else width="32" src="@/assets/business-case.png" />
        </td>
        <td><a target="_blank" :href="`https://finance.yahoo.com/quote/${item['ticker']}`">{{ item['ticker'] }}</a></td>
        <td>{{ item['industry'] }}</td>
        <td>{{ item['currentPrice'] }}</td>
        <td>{{ item['rs'] }}</td>
        <td>{{ item['mychoice'] }}</td>
      </tr>
    </tbody>
  </table>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { IRowViewModel } from '../views/PageReport/PageReport.types';

export default defineComponent({
  props: {
    caption: String,
    stocks: Array as PropType<IRowViewModel[]>,
  },
});
</script>
