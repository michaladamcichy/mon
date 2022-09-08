export const api = {};

api.url = `https://150.254.30.225:443/api/algorithm`;
// api.headers = {mode: 'cors'};
api.headers = {'Content-Type' : 'application/json'};
// api.headers = {'Content-Type' : 'application/json', Accept : 'application/json'};
// api.headers = {'Access-Control-Allow-Origin': '*'};

api.test = async () => {
    //console.log('running api test');

    const json = await fetch(`${api.url}`);
    const res = await json.json();
    //console.log(res);
};


const RANGES = [20, 30, 40, 50, 71, 100];

const parseCounts = (counts) => {
    return [counts[0], counts[1], 0, counts[2], 0, 0];
};

api.isConnected = async (ranges, counts, stations, units) => {
    const instance = {ranges/*: RANGES*/, counts/*: parseCounts(stationCounts)*/, stations, units}; //alert!!!
    
    const result = await fetch(`${api.url}/isConnected`, {method: 'POST', headers: api.headers, body: JSON.stringify(instance)});

    if(!result.ok) return undefined;
     
    const parsed = await result.json();
    return parsed;
};

api.algorithm = async (type, ranges, counts, stations, units, optimized = false) => {
    
    let _i = 1; //alert
    stations.forEach(station => {
        station.id = _i++;
    });
    //console.log(stations);
    const instance = {ranges/*: RANGES*/, counts/*: parseCounts(stationCounts)*/, stations, units, optimized};
    // console.log(units);
    //console.log(`${api.url}/${type}Algorithm`);
    const result = await fetch(`${api.url}/${type}Algorithm`, {method: 'POST', headers: api.headers, body: JSON.stringify(instance)});
    //alert todo obsluga bledow
    if(!result.ok) {
        //console.log('request failed');
        return;
    }

    //console.log('result');
    const res = await result.json();
    //console.log(res);
    return res;
}
