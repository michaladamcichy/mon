<script>
import {saveFile} from '../lib/file.js';
import {_wpUnits} from '../lib/wp.js';
export let instance;
export let index;
export let select;
export let remove;
export let selected;
export let duplicate;
export let load; //alert to jest bubel
export let percentageOfStations;

let input;
let fileInput;

const onFileSelected =(e)=>{
    //console.log('loading');
    let file = e.target.files[0];
    let reader = new FileReader();
    reader.readAsText(file);
    reader.onload = e => {
        try {
            const text = e.target.result;
            instance = JSON.parse(text);
            load(instance);
        }   catch(e) {
            //console.log(e);
        } 
    };
}

const loadWP = async () => {
    instance.units = instance.units.concat(_wpUnits);

    load(instance);
};

</script>

<div id="main" class="container" on:click={() => {select(instance);}}>
    <div class={`${instance == selected ? 'selectedRow ' : ""}controlsContainer form-group row d-flex justify-content-center align-items-center`}>
        <label class="col">{`${index + 1}.`}</label>
        <input type='text' class={`col btn ${selected == instance ? 'btn-light' : 'btn-secondary'}`}
            on:click={() => {
                if(instance != selected) {
                    input.blur();
                }
                select(instance);
            }}
            bind:value={instance.name}
            bind:this={input}
            placeholder='type instance name...'
            />
        <button class="col btn btn-primary" on:click={() => {fileInput.click();}}>Load</button>
        <button class="col btn btn-primary" on:click={() => {saveFile(instance.name, JSON.stringify(instance));}}>Save</button>
        <button class="col btn btn-primary" on:click={() => {loadWP();}}>WP</button>
        <button class="col btn btn-primary" on:click={() => {duplicate(instance);}}>Dupl.</button>
        <div class="col"></div>
        <button class="col btn btn-danger" on:click={() => {remove(instance);}}>X</button>
        <input style="display:none" type="file" accept="*" on:change={(e)=>onFileSelected(e)} bind:this={fileInput} >
    </div>
    <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
        
    </div>
</div>

<style>
    #main {
        margin-top: 5px;
        margin-bottom: 5px;
    }
    input {
        min-width: 100px;
        margin-right: 20px;
    }
    button {
        /* width: 30px; */
        height: 30px;
        font-size: 10px;
        font-weight: bold;
    }
    .row {
        margin-bottom: 5px;
        border-radius: 5px;
    }
    .selectedRow {
        background-color: lightgray;
    }
</style>