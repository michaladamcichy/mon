export const api = {};

api.url = `https://150.254.30.225:443/api/algorithm`;
// api.headers = {mode: 'cors'};
api.headers = {'Content-Type' : 'application/json'};
// api.headers = {'Content-Type' : 'application/json', Accept : 'application/json'};
// api.headers = {'Access-Control-Allow-Origin': '*'};

api.test = async () => {
    console.log('running api test');

    const json = await fetch(`${api.url}`);
    const res = await json.json();
    console.log(res);
};

api.isConnected = async (stationRanges, stationCounts, stations, units) => {
    const instance = {stationRanges, stationCounts, stations, units};
    
    const result = await fetch(`${api.url}/isConnected`, {method: 'POST', headers: api.headers, body: JSON.stringify(instance)});

    if(!result.ok) return undefined;
     
    const parsed = await result.json();
    return parsed;
};

api.algorithm = async (type, stationRanges, stationCounts, stations, units) => {
    const instance = {stationRanges, stationCounts, stations, units};
    const result = await fetch(`${api.url}/${type}Algorithm`, {method: 'POST', headers: api.headers, body: JSON.stringify(instance)});
    //alert todo obsluga bledow
    if(!result.ok) {
        console.log('request failed');
        return;
    }

    console.log('result');
    const res = await result.json();
    console.log(res);
    return res;
}

api.testPost = async (stationRanges, stations, units) => {
    const instance = {stationRanges, stationCounts: [1000,1000,1000], stations, units};
    const result = await fetch(api.url, {method: 'POST', headers: api.headers, body: JSON.stringify(instance)});
    const parsed = await result.json();
    console.log('post response:')
    console.log(parsed);
}

