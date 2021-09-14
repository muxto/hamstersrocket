"use strict";

var xmlhttp = new XMLHttpRequest();
var url = "report.json";

let rawData;
let stocksData;
let model;
let viewModel;

const tables = [{
        id: 'byRating',
        description: 'Stocks with maximum Buy and Strong Buy recommendations',
        n: 20,
        columns: [{
                caption: '#',
                valueField: 'num',
            }, {
                caption: 'Ticker',
                valueField: 'tickerLink',
            }, {
                caption: 'Industry',
                valueField: 'industry',
            }, {
                caption: 'Current price',
                valueField: 'currentPrice',
            }, {
                caption: 'Recommendations',
                valueField: 'rs',
            }
        ],
        sortField: 'strongBuyPlusBuy',
    }, {
        id: 'myChoice',
        description: 'Stocks with maximum difference between Target Prices and Current Price',
        n: 20,
        columns: [{
                caption: '#',
                valueField: 'num',
            }, {
                caption: 'Ticker',
                valueField: 'tickerLink',
            }, {
                caption: 'Industry',
                valueField: 'industry',
            }, {
                caption: 'Current price',
                valueField: 'currentPrice',
            }, {
                caption: 'Target Prices',
                valueField: 'targetPrices',
            }, {
                caption: 'My Take Profit',
                valueField: 'mychoice',
            }
        ],
        sortField: 'mychoicePercent',
    },

];

window.onload = function () {
    xmlhttp.open("GET", url, true);
    xmlhttp.send();
};

xmlhttp.onreadystatechange = function () {
    if (this.readyState == 4 && this.status == 200) {
        rawData = JSON.parse(this.responseText);

        updateDateRender();
        columnDescriptionsRender();

        model = getModel();
        viewModel = getViewModel();

        let tablesHtml = '';
        tables.forEach((scheme, i) => {

            scheme.id = "_table" + i;
            let table = tableRender(scheme);

            tablesHtml += '<div>' + table + '</div>';
        });
		
        document.getElementById('tables').innerHTML = tablesHtml;
    }
};

function updateDateRender() {
    document.getElementById('updateDate').innerHTML = new Date(rawData.UpdateDate).toLocaleString();
}

function columnDescriptionsRender() {

    let text = '';

    return;

    // viewColumns.forEach(({
    //         caption,
    //         description
    //     }) => {
    //     if (description !== '') {
    //         text += `<strong>${caption}</strong> - ${description}<br>`
    //     }
    // });
    //
    // document.getElementById('columsDescription').innerHTML = text;
}

function getModel() {

    model = [];

    stocksData = [...rawData.Stocks];

    for (var i = 0; i < stocksData.length; i++) {
        let row = stocksData[i];

        let rowModel = {};

        rowModel.t = row['Ticker'];
        rowModel.ind = row['Industry'];
        rowModel.c = row['CurrentPrice'];
        rowModel.pricel = row['TargetPriceLow'];
        rowModel.pricemean = row['TargetPriceMean'];
        rowModel.pricemedian = row['TargetPriceMedian'];
        rowModel.pricem = (rowModel.pricemean + rowModel.pricemedian) / 2;
        rowModel.priceh = row['TargetPriceHigh'];

        if (rowModel.c == 0) {
            rowModel.percentl = 0;
            rowModel.percentmean = 0;
            rowModel.percentmedian = 0;
            rowModel.percentm = 0;
            rowModel.percenth = 0;
        } else {
            rowModel.percentl = rowModel.pricel / rowModel.c * 100 - 100;
            rowModel.percentmean = rowModel.pricemean / rowModel.c * 100 - 100;
            rowModel.percentmedian = rowModel.pricemedian / rowModel.c * 100 - 100;
            rowModel.percentm = (rowModel.percentmean + rowModel.percentmedian) / 2;
            rowModel.percenth = rowModel.priceh / rowModel.c * 100 - 100;
        }

        rowModel.strongBuy = row['StrongBuy'];
        rowModel.buy = row['Buy'];
        rowModel.hold = row['Hold'];
        rowModel.sell = row['Sell'];
        rowModel.strongSell = row['StrongSell'];

        let rtn = rowModel.strongBuy +
            rowModel.buy +
            rowModel.hold +
            rowModel.sell +
            rowModel.strongSell;
        if (rtn == 0) {
            rowModel.rt = 0;
        } else {
            rowModel.rt = (rowModel.strongBuy * 1 +
                rowModel.buy * 2 +
                rowModel.hold * 3 +
                rowModel.sell * 4 +
                rowModel.strongSell * 5) / rtn;
        }

        rowModel.mychoice = getMyChoice(
                rowModel.rt,
                rowModel.pricel,
                rowModel.priceh,
                rowModel.percentmean,
                rowModel.percentmedian);

        model.push(rowModel);
    }
    return model;

    function getMyChoice(rt, pricel, priceh, percentmean, percentmedian) {

        if (rt === 0 || rt > 3)
            return 0;

        //if (pricel === priceh)
        //return 0;

        let percentm = (percentmean + percentmedian) / 2;

        if (percentm < 10)
            return 0;

        let c = percentm * 0.9;

        return c;
    }
}

function getViewModel() {

    viewModel = [];

    for (var i = 0; i < model.length; i++) {
        let row = model[i];

        let rowViewModel = {};

        rowViewModel.tickerLink = `<a target="_blank" href='https://finance.yahoo.com/quote/${row.t}'>${row.t}</a>`;
        rowViewModel.ticker = row.t;

        rowViewModel.industry = row.ind === null ? '' : row.ind;

        rowViewModel.currentPrice = row.c == 0 ? '' : parseFloat(`${row.c.toFixed(2)}`);

        if (row.c == 0 || row.pricel == 0) {
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
            rowViewModel.targetPrices = row.pricel;
            if (row.pricel !== row.priceh) {
                rowViewModel.targetPrices += ` ~ ${row.pricem.toFixed(2)} ~ ${row.priceh}`;
            }
            rowViewModel.priceh = row.priceh;

            rowViewModel.targetPercents = row.percentl.toFixed(2);
            if (row.percentl !== row.percenth) {
                rowViewModel.targetPercents += ` / ${row.percentm.toFixed(2)} / ${row.percenth.toFixed(2)}`;
            }
            rowViewModel.targetPercents = `x ${rowViewModel.targetPercents} %`;
            rowViewModel.percenth = row.percenth;

            rowViewModel.rs = `${row.strongBuy}, ${row.buy}, ${row.hold}, ${row.sell}, ${row.strongSell}`;
            rowViewModel.strongBuy = row.strongBuy;
            rowViewModel.strongBuyPlusBuy = row.strongBuy + row.buy;

            rowViewModel.rt = `${row.rt.toFixed(2)}`;

            if (row.mychoice === 0) {
                rowViewModel.mychoice = '';
                rowViewModel.mychoicePercent = '';
            } else {
                let p = row.c + row.c * (row.mychoice / 100);
                rowViewModel.mychoice = `${p.toFixed(2)} ( ${row.mychoice.toFixed(2)} %)`;
                rowViewModel.mychoicePercent = row.mychoice;
            }
        }
        viewModel.push(rowViewModel);
    }
    return viewModel;
}

function tableRender(scheme) {

    // headers

    let tableString = `<table id="${scheme.id}"><caption>${scheme.description}</caption><tr>`;

    scheme.columns.forEach(({
            caption
        }) => {
        tableString += `<th>${caption}</th>`
    });
    tableString += '</tr>';

    tableString += '<tr>'
    //viewColumns.forEach(({
    //        id,
    //        sortField,
    //        sortFieldCaption,
    //        buttons
    //    }) => {
    //    tableString += "<td>";
    //
    //    if (buttons) {
    //        buttons.forEach((item) => {
    //            tableString += `<button type="button" sort-field="${item.sortField}">Order by ${item.caption}</button>`
    //        });
    //    }
    //
    //    tableString += "</td>";
    //
    //});
    tableString += '</tr>';

    let sorted = sortByField(scheme.sortField);

    // fields

    // for (var i = 0; i < sorted.length; i++) {
    for (var i = 0; i < scheme.n; i++) {

        sorted[i].num = i + 1;

        tableString += '<tr>';

        scheme.columns.forEach(({
                valueField
            }) => {

            let row = sorted[i];

            if (valueField === 'targetPrices') {
                tableString += `<td>${row[valueField]}</td>`;
            } else {
                tableString += `<td>${row[valueField]}</td>`;
            }
        });
    }

    tableString += '</tr>';
    tableString += '</table>';

    return tableString;
};

function compare(a, b) {

    if (a > b) {
        return 1;
    }
    if (a < b) {
        return -1;
    }

    return 0;
}

function sortByField(field) {
    let emptyRows = viewModel.filter((item) => item[field] === '');
    let rows = viewModel.filter((item) => item[field] !== '');

    rows = rows.sort(function ({
                [field]: a
            }, {
                [field]: b
            }) {
        if (field === 'ticker' ||
            field === 'industry' ||
            field === 'rt' ||
            field === 'currentPrice') {
            return compare(a, b);
        } else {
            return compare(b, a);
        }
    });

    return rows.concat(emptyRows);
}

function addListeners() {
    const sortButtons = document.querySelectorAll('[sort-field]');
    Array.from(sortButtons).forEach((button) => {
        button.addEventListener('click', (event) => {
            sortField = event.target.getAttribute('sort-field');
            tableRender();
        });
    });
};
