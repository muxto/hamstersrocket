"use strict";

var xmlhttp = new XMLHttpRequest();
var url = "report.json";
let sortField = 'mychoice';
let rawData;
let stocksData;
let model;

const viewFields = [{
        name: 'ticker',
        caption: 'Ticker',
        description: 'Ticker',
        sortField: 't',
        sortFieldCaption: 'Ticker'
    }, {
        name: 'industry',
        caption: 'Industry',
        description: 'Industry',
        sortField: 'ind',
        sortFieldCaption: 'Industry'
    }, {
        name: 'currentPrice',
        caption: 'Current price',
        description: 'Stock current price',
        sortField: 'c',
        sortFieldCaption: 'Current price'
    }, {
        name: 'targetPrices',
        caption: 'Target prices',
        description: 'Low - median -  high',
        sortField: 'priceh',
        sortFieldCaption: 'High target price'
    }, {
        name: 'targetPercent',
        caption: 'Percent to current price',
        description: 'Low - median -  high',
        sortField: 'percenth',
        sortFieldCaption: 'High percent'
    }, {
        name: 'rs',
        caption: 'Recommendations',
        description: 'Count of StrongBuy, Buy, Hold, Sell and StrongSell recomendations',
        sortField: 'strongBuy',
        sortFieldCaption: 'Strong buy'
    }, {
        name: 'rt',
        caption: 'Recommendation trend',
        description: 'Mean value of StrongBuy(1), Buy(2), Hold(3), Sell(4) and StrongSell(5)',
        sortField: 'rt',
        sortFieldCaption: 'Recommendation trend'
    }, {
        name: 'mychoice',
        caption: 'My choice',
        description: 'I would set take profit to 90% of mean target prices, when RecommendationTrend < 3',
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

        rowModel.percentl = rowModel.pricel / rowModel.c * 100 - 100;
        rowModel.percentmean = rowModel.pricemean / rowModel.c * 100 - 100;
        rowModel.percentmedian = rowModel.pricemedian / rowModel.c * 100 - 100;
        rowModel.percentm = (rowModel.percentmean + rowModel.percentmedian) / 2;
        rowModel.percenth = rowModel.priceh / rowModel.c * 100 - 100;

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
}

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

function tableRender() {

    model = model.sort(sortByField);

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

    for (var i = 0; i < model.length; i++) {
        tableString += '<tr>';

        viewFields.forEach(({
                name
            }) => {

            let row = model[i];
            let val;

            if (name === 'ticker') {
                val = `<a href='https://finance.yahoo.com/quote/${row.t}'>${row.t}</a>`;
            }
            if (name === 'industry') {
                if (row.ind === null) {
                    val = '';
                } else {
                    val = row.ind;
                }
            }
            if (name === 'currentPrice') {
                val = row.c;
            }
            if (name === 'targetPrices') {
                val = row.pricel;
                if (row.pricel !== row.priceh) {
                    val += ` - ${row.pricem.toFixed(2)} - ${row.priceh}`;
                }
            }
            if (name === 'targetPercent') {
                val = row.percentl.toFixed(2);
                if (row.percentl !== row.percenth) {
                    val += ` - ${row.percentm.toFixed(2)} - ${row.percenth.toFixed(2)}`;
                }
                val = `x ${val} %`;
            }
            if (name === 'rs') {
                val = `${row.strongBuy}, ${row.buy}, ${row.hold}, ${row.sell}, ${row.strongSell}`;
            }
            if (name === 'rt') {
                val = row.rt;
            }
            if (name === 'mychoice') {
                if (row.mychoice === 0) {
                    val = 0;
                } else {
                    let p1 = row.pricem * 0.9;
                    let p2 = row.percentm * 0.9;
                    val = `${p1.toFixed(2)} x ${p2.toFixed(2)} %`;
                }
            }

            tableString += `<td>${val}</td>`;
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

function sortByField({
    [sortField]: a
}, {
    [sortField]: b
}) {
    if (sortField === 't' ||
        sortField === 'ind' ||
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
