<template>
  <div class="container">
    <h1 align="center">Hamster's Rocket!</h1>
    <h3 align="center">I hope it helps to find best-expectations-profit stocks</h3>
    <br>
    <div>
      Just-for-fun project. <a href="https://www.tinkoff.ru/invest/social/profile/muxto/">Here I am</a>. <a href="https://github.com/muxto/hamstersrocket">Github</a>
    </div>

    <div class="mb-3">This table shows recommendation targets by <a href='https://finnhub.io/'>Finnhub Stock API</a> and target prices by <a href='https://tipranks.com/'>TipRanks</a> for stocks available at <a href='https://www.tinkoff.ru/invest/'>Tinkoff Investing</a></div>

    <h2 class="mb-3">{{ updatedate }}</h2>

    <stock-table
      caption="Stocks with maximum Buy and Strong Buy recommendations"
      :stocks="strongBuyPlusBuyRecs"
    />

    <stock-table
      caption="Stocks with maximum difference between Target Prices and Current Price"
      :stocks="mychoicePercentRecs"
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
import StockTable from '@/components/StockTable.vue';
import { IReport, IRowModel, IRowViewModel } from './PageReport.types';

export default defineComponent({
  name: 'PageReport',
  components: { StockTable },
  setup() {
    function getMyChoice(
      rt: number,
      pricel: number,
      priceh: number,
      percentmean: number,
      percentmedian: number,
    ) {
      if (rt === 0 || rt > 3) return 0;

      // if (pricel === priceh)
      // return 0;

      const percentm = (percentmean + percentmedian) / 2;

      if (percentm < 10) return 0;

      const c = percentm * 0.9;

      return c;
    }

    function compare(a: number, b: number) {
      if (a > b) {
        return 1;
      }
      if (a < b) {
        return -1;
      }

      return 0;
    }

    const reportData = ref<null | IReport>(null);

    onMounted(async () => {
      reportData.value = await getReport('report.json');
    });

    const updatedate = computed(() => {
      if (!reportData.value) return '';

      return new Date(reportData.value.UpdateDate).toLocaleString();
    });

    const model = computed(() => {
      if (!reportData.value) return [];

      const modelOutput = [] as IRowModel[];

      const stocksData = [...reportData.value.Stocks];

      for (let i = 0; i < stocksData.length; i += 1) {
        const row = stocksData[i];

        const rowModel = {} as IRowModel;

        rowModel.t = row.Ticker;
        rowModel.ind = row.Industry;
        rowModel.c = row.CurrentPrice;
        rowModel.pricel = row.TargetPriceLow;
        rowModel.pricemean = row.TargetPriceMean;
        rowModel.pricemedian = row.TargetPriceMedian;
        rowModel.pricem = (rowModel.pricemean + rowModel.pricemedian) / 2;
        rowModel.priceh = row.TargetPriceHigh;

        if (rowModel.c === 0) {
          rowModel.percentl = 0;
          rowModel.percentmean = 0;
          rowModel.percentmedian = 0;
          rowModel.percentm = 0;
          rowModel.percenth = 0;
        } else {
          rowModel.percentl = (rowModel.pricel / rowModel.c) * 100 - 100;
          rowModel.percentmean = (rowModel.pricemean / rowModel.c) * 100 - 100;
          rowModel.percentmedian = (rowModel.pricemedian / rowModel.c) * 100 - 100;
          rowModel.percentm = (rowModel.percentmean + rowModel.percentmedian) / 2;
          rowModel.percenth = (rowModel.priceh / rowModel.c) * 100 - 100;
        }

        rowModel.strongBuy = row.StrongBuy;
        rowModel.buy = row.Buy;
        rowModel.hold = row.Hold;
        rowModel.sell = row.Sell;
        rowModel.strongSell = row.StrongSell;

        const rtn = rowModel.strongBuy
            + rowModel.buy
            + rowModel.hold
            + rowModel.sell
            + rowModel.strongSell;
        if (rtn === 0) {
          rowModel.rt = 0;
        } else {
          rowModel.rt = (rowModel.strongBuy * 1
              + rowModel.buy * 2
              + rowModel.hold * 3
              + rowModel.sell * 4
              + rowModel.strongSell * 5) / rtn;
        }

        rowModel.mychoice = getMyChoice(
          rowModel.rt,
          rowModel.pricel,
          rowModel.priceh,
          rowModel.percentmean,
          rowModel.percentmedian,
        );

        modelOutput.push(rowModel);
      }

      return modelOutput;
    });

    const viewModel = computed(() => {
      if (!model.value.length) return [];

      const viewModelOutput = [] as IRowViewModel[];

      for (let i = 0; i < model.value.length; i += 1) {
        const row = model.value[i] as IRowModel;

        const rowViewModel = {} as IRowViewModel;

        rowViewModel.tickerLink = `<a target="_blank" href='https://finance.yahoo.com/quote/${row.t}'>${row.t}</a>`;
        rowViewModel.ticker = row.t;

        rowViewModel.industry = row.ind === null ? '' : row.ind;

        rowViewModel.currentPrice = row.c === 0 ? '' : `${row.c.toFixed(2)}`;

        if (row.c === 0 || row.pricel === 0) {
          rowViewModel.targetPrices = '';
          rowViewModel.priceh = '';
          rowViewModel.targetPercents = '';
          rowViewModel.percenth = '';
          rowViewModel.rs = '';
          rowViewModel.strongBuy = '';
          rowViewModel.strongBuyPlusBuy = '';
          rowViewModel.rt = '';
          rowViewModel.mychoice = '';
          rowViewModel.mychoicePercent = '';
        } else {
          rowViewModel.targetPrices = String(row.pricel);
          if (row.pricel !== row.priceh) {
            rowViewModel.targetPrices += ` ~ ${row.pricem.toFixed(2)} ~ ${row.priceh}`;
          }
          rowViewModel.priceh = String(row.priceh);

          rowViewModel.targetPercents = row.percentl.toFixed(2);
          if (row.percentl !== row.percenth) {
            rowViewModel.targetPercents += ` / ${row.percentm.toFixed(2)} / ${row.percenth.toFixed(2)}`;
          }
          rowViewModel.targetPercents = `x ${rowViewModel.targetPercents} %`;
          rowViewModel.percenth = String(row.percenth);

          rowViewModel.rs = `${row.strongBuy}, ${row.buy}, ${row.hold}, ${row.sell}, ${row.strongSell}`;
          rowViewModel.strongBuy = String(row.strongBuy);
          rowViewModel.strongBuyPlusBuy = String(row.strongBuy + row.buy);

          rowViewModel.rt = `${row.rt.toFixed(2)}`;

          if (row.mychoice === 0) {
            rowViewModel.mychoice = '';
            rowViewModel.mychoicePercent = '';
          } else {
            const p = row.c + row.c * (row.mychoice / 100);
            rowViewModel.mychoice = `${p.toFixed(2)} ( ${row.mychoice.toFixed(2)} %)`;
            rowViewModel.mychoicePercent = String(row.mychoice);
          }
        }

        viewModelOutput.push(rowViewModel);
      }
      return viewModelOutput;
    });

    function sortByField(field: string) {
      const emptyRows = viewModel.value.filter((item) => item[field] === '');
      let rows = viewModel.value.filter((item) => item[field] !== '');

      rows = rows.sort(({
        [field]: a,
      }, {
        [field]: b,
      }) => {
        if (field === 'ticker'
          || field === 'industry'
          || field === 'rt'
          || field === 'currentPrice') {
          return compare(Number(a), Number(b));
        }

        return compare(Number(b), Number(a));
      });

      return rows.concat(emptyRows);
    }

    function prepareTableData(field: string): IRowViewModel[] {
      const sorted = sortByField(field);

      return sorted.slice(0, 20);
    }

    const strongBuyPlusBuyRecs = computed(() => prepareTableData('strongBuyPlusBuy'));
    const mychoicePercentRecs = computed(() => prepareTableData('mychoicePercent'));

    return {
      updatedate,
      viewModel,
      strongBuyPlusBuyRecs,
      mychoicePercentRecs,
    };
  },
});
</script>
