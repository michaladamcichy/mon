export const api = {};

api.url = `http://localhost:5688/api/algorithm`;
// api.headers = {mode: 'cors'};
api.headers = {};
// api.headers = {'Access-Control-Allow-Origin': '*'};

api.test = async () => {
    console.log('running api test');

    const json = await fetch(`${api.url}`, {headers: api.headers});
    const res = await json.json();
    console.log(res);
};