const DATA = `
50 & (118.2, 5029) &(3.33, 182.72) & (82.2, 3700) & (3.26, 200.28)\\
75 & (151.9, 6341) & (5.47, 257.4) & (99.8, 4416) & (4.26, 174.81)\\
100 & (187.3, 7624) & (4.97, 227.46) & (122.9, 5330) & (4.72, 305.58)\\
125 & (214.3, 8736) & (6.33, 184.76) & (140.1, 6071) & (3.84, 204.09)\\
150 & (247.5, 10146) & (6.19, 334.11) & (157.9, 6840) & (4.75, 306.3)\\
175 & (274.7, 11124) & (5.01, 227.07) & (174.8, 7472) & (4.71, 154.83)\\`;


const addCostPlot = (coordinates) => `
\\addplot+[
    error bars/.cd,
    y dir=both,
    y explicit
] coordinates {
  ${coordinates}
 };
`;

const getCoordinates = data => {
    data = data.replace(/\\/g, '');
    let rows = data.split('\n');

    rows = rows.map(row => row.split(`&`));
    
    rows = rows.filter(row => row != '');
    rows = rows.map(row => {
        // console.log(row);
        let N = row[0];
        
        let [count1, sum1] = row[1].trim().replace("(", "").replace(")", "").split(", ").map(item => item.trim());
        let [stdCount1, stdSum1] = row[2].trim().replace("(", "").replace(")", "").split(", ").map(item => item.trim());
        let [count2, sum2] = row[3].trim().replace("(", "").replace(")", "").split(", ").map(item => item.trim());
        let [stdCount2, stdSum2] = row[4].trim().replace("(", "").replace(")", "").split(", ").map(item => item.trim());
        
        
        return [{N, count: count1, sum: sum1, stdCount: stdCount1, stdSum: stdSum1},
            {N, count: count2, sum: sum2, stdCount: stdCount2, stdSum: stdSum2}]; 
    });
    

    return [0,1].map(series => {
        return [
        rows.map(row => {
            return `(${row[series].N},${row[series].count}) += (-${row[series].stdCount},${row[series].stdCount})`
        }).join(' '),
        rows.map(row => {
            return `(${row[series].N},${row[series].sum}) += (-${row[series].stdSum},${row[series].stdSum})`
        }).join(' '),
        ];
    });
};

const fromCostTableToCostPlot = (data) => {

};

console.log(getCoordinates(DATA));