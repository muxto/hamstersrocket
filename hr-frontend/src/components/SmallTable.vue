<template>
  <table class="table table-striped table-hover caption-top">
    <caption>{{ caption }}</caption>
    <tbody>
      <tr>
        <th>Logo</th>
        <th>Company</th>
        <th>Current price</th>
        <!--<th>Recommendation trend</th>-->
      </tr>
      <tr v-for="(item, index) in stocks" :key="index">
        <td class="align-middle">
            <img class="table-logo" width="32" :src="item['logo']" />
        </td>
        <td class="align-middle">
            <div>{{ item['CompanyName'] }}</div>
            <div><small class="text-muted">{{ item['industry'] }}</small></div>
            <small class="text-muted">{{ item['ticker'] }}</small>
        </td>
        <td>
            <div>{{ item['currentPrice'] }}</div>
            <div><small class="text-muted">{{ item['rs'] }}</small></div>
            <span class="badge rounded-pill bg-success">{{ item['rt'] }}</span>
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

<style lang="scss" scoped>
.table-logo {
    border-radius: 50%;
}
</style>
