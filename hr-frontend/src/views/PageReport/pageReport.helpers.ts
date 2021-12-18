import { IReport, IRowModel, IRowViewModel } from './PageReport.types';

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

  const c = percentm;

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

function computeRowModel(report: IReport | null): IRowModel[] {
  if (!report) return [];

  const modelOutput = [] as IRowModel[];

  const stocksData = [...report.Stocks];

  for (let i = 0; i < stocksData.length; i += 1) {
    const row = stocksData[i];

    const rowModel = {} as IRowModel;

    rowModel.t = row.Ticker;
    rowModel.ind = row.Industry;
    rowModel.logo = row.Logo;
    rowModel.companyname = row.CompanyName;
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
}

function computeRowViewModel(rowModel: IRowModel[] | null): IRowViewModel[] {
  if (!rowModel) return [];

  const viewModelOutput = [] as IRowViewModel[];

  for (let i = 0; i < rowModel.length; i += 1) {
    const row = rowModel[i] as IRowModel;

    const rowViewModel = {} as IRowViewModel;

    rowViewModel.ticker = row.t;

    rowViewModel.industry = row.ind === null ? '' : row.ind;
    rowViewModel.logo = row.logo === null ? '' : row.logo;
    rowViewModel.CompanyName = row.companyname === null ? '' : row.companyname;

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
      rowViewModel.pricel = String(row.pricel);
      rowViewModel.pricem = String(row.pricem.toFixed(2));
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
        rowViewModel.mychoice = `${p.toFixed(2)}`;
        rowViewModel.mychoicePercent = `${row.mychoice.toFixed(2)}`;
      }
    }

    viewModelOutput.push(rowViewModel);
  }
  return viewModelOutput;
}

function sortByField(viewModel: IRowViewModel[], field: string): IRowViewModel[] {
  const emptyRows = viewModel.filter((item) => item[field] === '');
  let rows = viewModel.filter((item) => item[field] !== '');

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

function prepareTableData(viewModel: IRowViewModel[], field: string): IRowViewModel[] {
  const sorted = sortByField(viewModel, field);

  return sorted.slice(0, 20);
}

export {
  computeRowModel,
  computeRowViewModel,
  prepareTableData,
};
