<template>
    <div>
        <div class="alert alert-danger" role="alert" v-show="showErrorMessages" data-cy="error-div" id="errorMessages">
            <ul>
                <li :key="msg" v-for="msg in errors">
                    {{msg}}
                </li>
            </ul>
        </div>
        <div class="alert alert-success" role="alert" v-show="showSuccessMessages" data-cy="success-div"
             id="successMessages">
            <ul>
                <li :key="msg" v-for="msg in successMsgs">
                    {{msg}}
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Messages",
        props: {
            errors: {
                type: Array,
                default: () => []
            },
            successMsgs: {
                type: Array,
                default: () => []
            }
        },
        computed: {
            showErrorMessages() {
                return this.errors.length > 0;
            },
            showSuccessMessages() {
                return this.successMsgs.length > 0;
            }
        }
    }
</script>

<style scoped>

</style>