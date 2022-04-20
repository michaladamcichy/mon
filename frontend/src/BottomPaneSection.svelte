<script>
    export let title;
</script>

<div class="main col">
    <h4 class="row">{title}</h4>
    <div class="row">
        <slot></slot>
    </div>

</div>

<style>
    .main {
        border-right: 2px solid lightgrey;
        height: 24vh;
        margin: 5px;
    }

</style>