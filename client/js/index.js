"use strict";

var xmlhttp = new XMLHttpRequest();
var url = "report.json";
let sortField = 'mychoice';
let rawData;
let stocksData;
let model;
let viewModel;

const viewFields = [{
        caption: 'Ticker',
        description: 'Ticker',
        valueField: 'tickerLink',
        sortField: 'ticker',
        sortFieldCaption: 'Ticker'
    }, {
        caption: 'Industry',
        description: 'Industry',
        valueField: 'industry',
        sortField: 'industry',
        sortFieldCaption: 'Industry'
    }, {
        caption: 'Current price',
        description: 'Stock current price',
        valueField: 'currentPrice',
        sortField: 'currentPrice',
        sortFieldCaption: 'Current price'
    }, {
        caption: 'Target prices',
        description: 'Low - median -  high',
        valueField: 'targetPrices',
        sortField: 'priceh',
        sortFieldCaption: 'High target price'
    }, {
        caption: 'Percent to current price',
        description: 'Low - median -  high',
        valueField: 'targetPercents',
        sortField: 'percenth',
        sortFieldCaption: 'High percent'
    }, {
        caption: 'Recommendations',
        description: 'Count of StrongBuy, Buy, Hold, Sell and StrongSell recomendations',
        valueField: 'rs',
        sortField: 'strongBuy',
        sortFieldCaption: 'Strong buy'
    }, {
        caption: 'Recommendation trend',
        description: 'Mean value of StrongBuy(1), Buy(2), Hold(3), Sell(4) and StrongSell(5)',
        valueField: 'rt',
        sortField: 'rt',
        sortFieldCaption: 'Recommendation trend'
    }, {
        caption: 'My choice',
        description: 'I would set take profit to 90% of mean target prices, when RecommendationTrend < 3',
        valueField: 'mychoice',
        sortField: 'mychoice',
        sortFieldCaption: 'My choice percent'
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
        tableRender();
    }
};

function updateDateRender() {
    document.getElementById('updateDate').innerHTML = new Date(rawData.UpdateDate).toLocaleString();
}

function columnDescriptionsRender() {
    let text = '';
    viewFields.forEach(({
            caption,
            description
        }) => {
        text += `<strong>${caption}</strong> - ${description}<br>`
    });

    document.getElementById('columsDescription').innerHTML = text;
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

        if (pricel === priceh)
            return 0;

        let percentm = (percentmean + percentmedian) / 2;

        if (percentm < 110)
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

        rowViewModel.tickerLink = `<a href='https://finance.yahoo.com/quote/${row.t}'>${row.t}</a>`;
        rowViewModel.ticker = row.t;

        rowViewModel.industry = row.ind === null ? '' : row.ind;

        rowViewModel.currentPrice = row.c == 0 ? '' : row.c;

        if (row.c == 0 || row.pricel == 0) {
            rowViewModel.targetPrices = '';
            rowViewModel.priceh = '';
            rowViewModel.targetPercents = '';
            rowViewModel.percenth = '';
            rowViewModel.rs = '';
            rowViewModel.strongBuy = '';
            rowViewModel.rt = '';
            rowViewModel.mychoice = '';
        } else {

            rowViewModel.targetPrices = row.pricel;
            if (row.pricel !== row.priceh) {
                rowViewModel.targetPrices += ` - ${row.pricem.toFixed(2)} - ${row.priceh}`;
            }
            rowViewModel.priceh = row.priceh;

            rowViewModel.targetPercents = row.percentl.toFixed(2);
            if (row.percentl !== row.percenth) {
                rowViewModel.targetPercents += ` - ${row.percentm.toFixed(2)} - ${row.percenth.toFixed(2)}`;
            }
            rowViewModel.targetPercents = `x ${rowViewModel.targetPercents} %`;
            rowViewModel.percenth = row.percenth;

            rowViewModel.rs = `${row.strongBuy}, ${row.buy}, ${row.hold}, ${row.sell}, ${row.strongSell}`;
            rowViewModel.strongBuy = row.strongBuy;

            rowViewModel.rt = `${row.rt.toFixed(2)}`;

            if (row.mychoice === 0) {
                rowViewModel.mychoice = '';
            } else {
                let p1 = row.pricem * 0.9;
                let p2 = row.percentm * 0.9;
                rowViewModel.mychoice = `${p1.toFixed(2)} x ${p2.toFixed(2)} %`;
            }
        }
        viewModel.push(rowViewModel);
    }
    return viewModel;
}

function tableRender() {

    let sorted = sortByField();

    let tableString = '<table><tr>';
    viewFields.forEach(({
            caption,
            description
        }) => {
        tableString += `<th title="${description}">${caption}</th>`
    });
    tableString += '</tr>';

    tableString += '<tr>'
    viewFields.forEach(({
            sortField,
            sortFieldCaption
        }) => {
        tableString += `<td><button type="button" sort-field="${sortField}">Order by ${sortFieldCaption}</button></td>`;
    });
    tableString += '</tr>';

    for (var i = 0; i < sorted.length; i++) {
        tableString += '<tr>';

        viewFields.forEach(({
                valueField
            }) => {

            let row = sorted[i];

            tableString += `<td>${row[valueField]}</td>`;
        });
    }

    tableString += '</tr>';
    tableString += '</table>';

    document.getElementById('stocksTable').innerHTML = tableString;

    addListeners();
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

function sortByField() {
    let emptyRows = viewModel.filter((item) => item[sortField] === '');
    let rows = viewModel.filter((item) => item[sortField] !== '');

    rows = rows.sort(customSort);
    return rows.concat(emptyRows);
}

function customSort({
    [sortField]: a
}, {
    [sortField]: b
}) {

    if (sortField === 'ticker' ||
        sortField === 'industry' ||
        sortField === 'rt') {
        return compare(a, b);
    } else {
        return compare(b, a);
    }
};

function addListeners() {
    const sortButtons = document.querySelectorAll('[sort-field]');
    Array.from(sortButtons).forEach((button) => {
        button.addEventListener('click', (event) => {
            sortField = event.target.getAttribute('sort-field');
            tableRender();
        });
    });
};
