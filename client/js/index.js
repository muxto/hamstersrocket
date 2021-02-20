"use strict";

var xmlhttp = new XMLHttpRequest();
var url = "report.json";
let sortField = 'RecommendationTrend';
let data;
let zeros;
let noneZeros;

const fields = [{
        name: 'Ticker'
    }, {
        name: 'CurrentPrice'
    }, {
        name: 'TargetPriceHigh'
    }, {
        name: 'TargetPriceHighPercent'
    }, {
        name: 'TargetPriceMean'
    }, {
        name: 'TargetPriceMeanPercent'
    }, {
        name: 'TargetPriceMedian'
    }, {
        name: 'TargetPriceMedianPercent'
    }, {
        name: 'TargetPriceLow'
    }, {
        name: 'TargetPriceLowPercent'
    }, {
        name: 'StrongBuy'
    }, {
        name: 'Buy'
    }, {
        name: 'Hold'
    }, {
        name: 'Sell'
    }, {
        name: 'StrongSell'
    }, {
        name: 'RecommendationTrend'
    },
];

const viewFields = [{
        caption: 'Ticker',
        description: 'Ticker',
        shortName: 't',
        sortField: 'Ticker',
        sortFieldCaption: 'Ticker'
    }, {
        caption: 'Current price',
        description: 'Stock current price',
        shortName: 'c',
        sortField: 'CurrentPrice',
        sortFieldCaption: 'Current price'
    }, {
        caption: 'Target prices',
        description: 'Low - median -  high',
        shortName: 'price',
        sortField: 'TargetPriceHigh',
        sortFieldCaption: 'High target price'
    }, {
        caption: 'Percent to current price',
        description: 'Low - median -  high',
        shortName: 'percent',
        sortField: 'TargetPriceHighPercent',
        sortFieldCaption: 'High percent'
    }, {
        caption: 'Recommendations',
        description: 'Count of StrongBuy, Buy, Hold, Sell and StrongSell recomendations',
        shortName: 'rts',
        sortField: 'StrongBuy',
        sortFieldCaption: 'Strong buy'
    }, {
        caption: 'Recommendation trend',
        description: 'Mean value of StrongBuy(1), Buy(2), Hold(3), Sell(4) and StrongSell(5)',
        shortName: 'rt',
        sortField: 'RecommendationTrend',
        sortFieldCaption: 'Recommendation trend'
    }, {
        caption: 'My choice',
        description: 'I would set take profit to 90% of mean target prices, when RecommendationTrend < 3',
        shortName: 'mychoice',
        sortField: 'MyChoice',
        sortFieldCaption: 'My choice percent'
    },
];

window.onload = function () {
    xmlhttp.open("GET", url, true);
    xmlhttp.send();
};

function updateDateRender() {
    document.getElementById('updateDate').innerHTML = new Date(data.UpdateDate).toLocaleString();
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

function tableRender() {

    noneZeros = noneZeros.sort(sortByField);
    const sortedData = noneZeros.concat(zeros);

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

    let val;
    let t;

    let c;
    let pricel;
    let pricemean;
    let pricemedian;
    let pricem;
    let priceh;

    let percentl;
    let percentmean;
    let percentmedian;
    let percentm;
    let percenth;

    let rt;
    let strongBuy;
    let buy;
    let hold;
    let sell;
    let strongSell;

    let mychoice;

    for (var i = 0; i < sortedData.length; i++) {
        tableString += '<tr>';

        viewFields.forEach(({
                shortName
            }) => {

            let row = sortedData[i];
            t = row['Ticker'];
            c = row['CurrentPrice'];
            pricel = row['TargetPriceLow'];
            pricemean = row['TargetPriceMean'];
            pricemedian = row['TargetPriceMedian'];
            pricem = (pricemean + pricemedian) / 2;
            priceh = row['TargetPriceHigh'];
            percentl = row['TargetPriceLowPercent'];
            percentmean = row['TargetPriceMeanPercent'];
            percentmedian = row['TargetPriceMedianPercent'];
            percentm = (percentmean + percentmedian) / 2;
            percenth = row['TargetPriceHighPercent'];
            rt = row['RecommendationTrend'];
            strongBuy = row['StrongBuy'];
            buy = row['Buy'];
            hold = row['Hold'];
            sell = row['Sell'];
            strongSell = row['StrongSell'];
            mychoice = row['MyChoice'];

            if (shortName === 't') {
                val = `<a href='https://finance.yahoo.com/quote/${t}'>${t}</a>`;
            }
            if (shortName === 'c') {
                val = c;
            }
            if (shortName === 'price') {
                val = pricel;
                if (pricel !== priceh) {
                    val += ` - ${pricem.toFixed(2)} - ${priceh}`;
                }
            }
            if (shortName === 'percent') {
                val = percentl.toFixed(2);
                if (percentl !== percenth) {
                    val += ` - ${percentm.toFixed(2)} - ${percenth.toFixed(2)}`;
                }
                val = `x ${val} %`;
            }
            if (shortName === 'rts') {
                val = `${strongBuy}, ${buy}, ${hold}, ${sell}, ${strongSell}`;
            }
            if (shortName === 'rt') {
                val = rt;
            }
            if (shortName === 'mychoice') {
                if (mychoice === 0) {
                    val = 0;
                } else {
                    let p1 = pricem * 0.9;
                    let p2 = percentm * 0.9;
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
    if (sortField === 'RecommendationTrend' ||
        sortField === 'Ticker') {
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

function addMyChoiceColumn(stocksData) {
    let row;
    for (var i = 0; i < stocksData.length; i++) {
        row = stocksData[i];
        row.MyChoice = getMyChoice(row);
    }
}

function getMyChoice(row) {

    let rt = row['RecommendationTrend'];
    if (rt === 0 || rt > 3)
        return 0;

    let pricel = row['TargetPriceLow'];
    let priceh = row['TargetPriceHigh'];

    if (pricel === priceh)
        return 0;

    let percentmean = row['TargetPriceMeanPercent'];
    let percentmedian = row['TargetPriceMedianPercent'];

    let percentm = (percentmean + percentmedian) / 2;

    if (percentm < 110)
        return 0;

    let c = percentm * 0.9;

    return c;
}

xmlhttp.onreadystatechange = function () {
    if (this.readyState == 4 && this.status == 200) {
        data = JSON.parse(this.responseText);

        updateDateRender();
        columnDescriptionsRender();

        const stocksData = [...data.Stocks];

        addMyChoiceColumn(stocksData);

        zeros = stocksData.filter((item) => item.RecommendationTrend === 0);
        noneZeros = stocksData.filter((item) => item.RecommendationTrend !== 0);

        tableRender();
    }

};
