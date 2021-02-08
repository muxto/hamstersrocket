var xmlhttp = new XMLHttpRequest();
var url = "/report.json";
let sortField = 'RecommendationTrend';
let data;
let zeros;
let noneZeros;

const fields = [
{ name: 'Ticker' },
{ name: 'CurrentPrice'              },
{ name: 'TargetPriceHigh'           },
{ name: 'TargetPriceHighPercent'    },
{ name: 'TargetPriceMean'           },
{ name: 'TargetPriceMeanPercent'    },
{ name: 'TargetPriceMedian'         },
{ name: 'TargetPriceMedianPercent'  },
{ name: 'TargetPriceLow'            },
{ name: 'TargetPriceLowPercent'     },
{ name: 'StrongBuy'                 },
{ name: 'Buy'                       },
{ name: 'Hold'                      },
{ name: 'Sell'                      },
{ name: 'StrongSell'                },
{ name: 'RecommendationTrend'       },
];

const viewFields = [
{ caption: 'Ticker', shortName: 't', sortField: 'Ticker', sortFieldCaption: 'Ticker'},
{ caption: 'Current price', shortName: 'c', sortField: 'CurrentPrice', sortFieldCaption: 'Current price'},
{ caption: 'Target prices low - high', shortName: 'price', sortField: 'TargetPriceHigh', sortFieldCaption: 'High target price'},
{ caption: 'Percent to current price', shortName: 'percent', sortField: 'TargetPriceHighPercent', sortFieldCaption: 'High percent'},
{ caption: 'Recommendation trend', shortName: 'rt', sortField: 'RecommendationTrend', sortFieldCaption: 'Recommendation trend'},
];

window.onload = function() {
  xmlhttp.open("GET", url, true);
  xmlhttp.send();  
};

function tableRender() {
  document.getElementById('updateDate').innerHTML = new Date(data.UpdateDate).toLocaleString();

  noneZeros = noneZeros.sort(sortByField);
  const sortedData = noneZeros.concat(zeros);


  let tableString = '<table><tr>';
  viewFields.forEach(({ caption }) => {
	tableString += `<th>${caption}</th>`
  });
  tableString += '</tr>';
  
  tableString += '<tr>'
  viewFields.forEach(({ sortField, sortFieldCaption }) => {
	tableString += `<td><button type="button" sort-field="${sortField}">Order by ${sortFieldCaption}</button></td>`;
  });
  tableString += '</tr>';
    
	
	
  let val;
  let t;
  let c;
  let pricel;
  let priceh;
  let percentl;
  let percenth;
  let rt;
  
  for(var i = 0; i < sortedData.length; i++) {
	tableString += '<tr>'; 
	  
    viewFields.forEach(({ shortName }) => {
	  
	  let row = sortedData[i];
	  t = row['Ticker'];
	  c = row['CurrentPrice']
      pricel = row['TargetPriceLow'];
      priceh = row['TargetPriceHigh']
      percentl = row['TargetPriceLowPercent'];
      percenth = row['TargetPriceHighPercent'];
      rt = row['RecommendationTrend'];
	  
	  if (shortName === 't') 	{
	  	val = `<a href='https://finance.yahoo.com/quote/${t}'>${t}</a>`;
	  }
	  if (shortName === 'c') 	{
	  	val = c;
	  }
	  if (shortName === 'price') {
		val = pricel;
		if (pricel !== priceh) {
		  val += ` - ${priceh}`;
		}
	  }
	  if (shortName === 'percent') 	{  	
		val = percentl.toFixed(2);
		if (percentl !== percenth) {
		  val += ` - ${percenth.toFixed(2)}`;
		}
		val = `x ${val} %`;
	  }
	  if (shortName === 'rt') 	{
	  	val = rt;
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

function sortByField({[sortField]: a}, {[sortField]: b}) {
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

xmlhttp.onreadystatechange = function() {
    if (this.readyState == 4 && this.status == 200) {
        data = JSON.parse(this.responseText);

		const stocksData = [...data.Stocks];	 
		zeros = stocksData.filter((item) => item.RecommendationTrend === 0);
		noneZeros = stocksData.filter((item) => item.RecommendationTrend !== 0);
		
        tableRender();
    }
};
