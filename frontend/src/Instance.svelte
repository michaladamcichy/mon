<script>

export let instance;
export let index;
export let select;
export let remove;
export let selected;
export let duplicate;

</script>

<div id="main" class="container">
    <div class={`${instance == selected ? 'selectedRow ' : ""}controlsContainer form-group row d-flex justify-content-center align-items-center`}>
        <label class="col">{`${index + 1}.`}</label>
        <div class={`col btn ${selected == instance ? 'btn-light' : 'btn-secondary'}`} on:click={() => select(instance)}>Instance</div>
        <div class="col"></div>
        <button class="btn btn-primary" on:click={() => {duplicate(instance);}}>D</button>
        <div class="col"></div>
        <button class="btn btn-danger" on:click={() => {remove(instance);}}>X</button>
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
        width: 30px;
        height: 30px;
        font-size: 10px;
        font-weight: bold;
    }
    .row {
        margin-bottom: 5px;
    }
    .selectedRow {
        background-color: lightgray;
    }
</style>