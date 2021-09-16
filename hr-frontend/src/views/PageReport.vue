<template>
  <div>
    <h1 align="center">Hamster's Rocket!</h1>
    <br>
    <h3 align="center">I hope it helps to find best-expectations-profit stocks</h3>
    <br>
    <div align="center">
      Just-for-fun project. <a href="https://www.tinkoff.ru/invest/social/profile/muxto/">Here I am</a>. <a href="https://github.com/muxto/hamstersrocket">Github</a>
    </div>

    <div>This table shows recommendation targets by <a href='https://finnhub.io/'>Finnhub Stock API</a> and target prices by <a href='https://tipranks.com/'>TipRanks</a> for stocks available at <a href='https://www.tinkoff.ru/invest/'>Tinkoff Investing</a></div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted } from 'vue';
import getReport from '../api';

export default defineComponent({
  name: 'PageReport',
  setup() {
    onMounted(async () => {
      const reportData = await getReport('report.json');
      console.log(reportData);
    });
  },
});
</script>
