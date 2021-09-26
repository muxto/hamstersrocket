interface IStock {
  Buy: number;
  CurrentPrice: number;
  Hold: number;
  Industry: string;
  Sell: number;
  StrongBuy: number;
  StrongSell: number;
  TargetPriceHigh: number;
  TargetPriceLow: number;
  TargetPriceMean: number;
  TargetPriceMedian: number;
  Ticker: string;
}

interface IReport {
  Stocks: IStock[]
  UpdateDate: string;
}

interface IRowModel {
  t: string;
  ind: string;
  c: number;
  pricel: number;
  pricemean: number;
  pricemedian: number;
  pricem: number;
  priceh: number;
  percentl: number;
  percentmean: number;
  percentmedian: number;
  percentm: number;
  percenth: number;
  strongBuy: number;
  buy: number;
  hold: number;
  sell: number;
  strongSell: number;
  rt: number;
  mychoice: number;
}

interface IRowViewModel {
  [key: string]: string;
}

// interface IRowViewModel {
//   tickerLink: string;
//   ticker: string;
//   industry: string;
//   currentPrice: string;
//   targetPrices: string;
//   priceh: string;
//   targetPercents: string;
//   percenth: string;
//   rs: string;
//   strongBuy: string;
//   strongBuyPlusBuy: string;
//   rt: string;
//   mychoice: string;
//   mychoicePercent: string;
// }

export {
  IStock,
  IReport,
  IRowModel,
  IRowViewModel,
};
