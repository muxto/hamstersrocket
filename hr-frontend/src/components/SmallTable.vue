<template>
  <table class="table table-striped table-hover caption-top">
    <caption>{{ caption }}</caption>
    <tbody>
      <tr>
        <th>Logo</th>
        <th>Ticker</th>
        <th>Current price</th>
        <th>Target prices</th>
      </tr>
      <tr v-for="(item, index) in stocks" :key="index">
        <td><img width="32" :src="item['logo']" /></td>
        <td>
            <div>{{ item['CompanyName'] }}</div>
            <a target="_blank" :href="`https://finance.yahoo.com/quote/${item['ticker']}`">{{ item['ticker'] }}</a>
        </td>
        <td>{{ item['currentPrice'] }}</td>
        <td>
            <span class="text-danger">{{ item['pricel'] }}</span>
            ~
            <span class="text-primary">{{ item['pricem'] }}</span>
            ~
            <span class="text-success">{{ item['priceh'] }}</span>
        </td>
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
