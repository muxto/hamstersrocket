<template>
  <table class="table table-striped table-hover caption-top">
    <caption>{{ caption }}</caption>
    <tbody>
      <tr>
        <th>#</th>
        <th>Ticker</th>
        <th>Industry</th>
        <th>Current price</th>
        <th>Recommendations</th>
      </tr>
      <tr v-for="(item, index) in stocks" :key="index">
        <td>{{ index + 1 }}</td>
        <td><a target="_blank" :href="`https://finance.yahoo.com/quote/${item['ticker']}`">{{ item['ticker'] }}</a></td>
        <td>{{ item['industry'] }}</td>
        <td>{{ item['currentPrice'] }}</td>
        <td>{{ item['rs'] }}</td>
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
