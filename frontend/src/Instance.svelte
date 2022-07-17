<script>

export let instance;
export let index;
export let select;

</script>

<div id="main" class="container">
    <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
        <label class="col">{`${index + 1}.`}</label>
        <button class="col btn btn-primary" on:click={() => select(instance)}>Instance</button>
        <div class="col"></div>
        <div class="col"></div>
        <button id="removeButton" class="btn btn-danger" on:click={() => {}}>X</button>
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
    #removeButton {
        width: 30px;
        height: 30px;
        font-size: 10px;
        font-weight: bold;
    }
    .row {
        margin-bottom: 5px;
    }
</style>