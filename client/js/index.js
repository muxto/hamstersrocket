var xmlhttp = new XMLHttpRequest();
var url = "/report.json";
let sortField = 'RecommendationTrend';
let data;
let zeros;
let noneZeros;

const fields = [
{ name: 'Ticker',                       measureUnit: 'value'    },
{ name: 'CurrentPrice',                 measureUnit: 'value'    },
{ name: 'TargetPriceHigh',              measureUnit: 'value'    },
{ name: 'TargetPriceHighPercent',       measureUnit: 'percent'  },
{ name: 'TargetPriceMean',              measureUnit: 'value'    },
{ name: 'TargetPriceMeanPercent',       measureUnit: 'percent'  },
{ name: 'TargetPriceMedian',            measureUnit: 'value'    },
{ name: 'TargetPriceMedianPercent',     measureUnit: 'percent'  },
{ name: 'TargetPriceLow',               measureUnit: 'value'    },
{ name: 'TargetPriceLowPercent',        measureUnit: 'percent'  },
{ name: 'StrongBuy',                    measureUnit: 'value'    },
{ name: 'Buy',                          measureUnit: 'value'    },
{ name: 'Hold',                         measureUnit: 'value'    },
{ name: 'Sell',                         measureUnit: 'value'    },
{ name: 'StrongSell',                   measureUnit: 'value'    },
{ name: 'RecommendationTrend',          measureUnit: 'value'    },
];

window.onload = function() {
  xmlhttp.open("GET", url, true);
  xmlhttp.send();  
};

function tableRender() {
  document.getElementById('updateDate').innerHTML = new Date(data.UpdateDate).toLocaleString();

  let tableString = '<table><tr>';
  fields.forEach(({ name }) => {
	tableString += `<th><button type="button" data-field="${name}">${name}</button></th>`;
  });
  tableString += '</tr>';
    
  noneZeros = noneZeros.sort(sortByField);
  const sortedData = noneZeros.concat(zeros);
  
  for(var i = 0; i < sortedData.length; i++) {
    tableString += '<tr>'
	fields.forEach(({ name, measureUnit }) => {
		const val = (measureUnit === 'percent') 
			? `x ${sortedData[i][name]} %` 
			: sortedData[i][name];
		tableString += `<td>${val}</td>`;
	});
    tableString += '</tr>';
  }

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
	if (sortField === 'RecommendationTrend') {
		return compare(a, b);
	} else {
		return compare(b, a);
	}
};

function addListeners() {
	const sortButtons = document.querySelectorAll('[data-field]');
	Array.from(sortButtons).forEach((button) => {
		button.addEventListener('click', (event) => {
			sortField = event.target.getAttribute('data-field');
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
