<template>
    <div>
        <div style="text-align:center">
            <p class="centered"> {{message}}</p>
            <div class="spinner">
                <div class="bubble-1"></div>
                <div class="bubble-2"></div>
            </div>
        </div>
    </div>
</template>
<script>
  export default {
    name: 'Loader',
    props: {
      message:{
        type: String
      }
    }
  }
</script>

<style>
    
    .centered {
        margin: 0 auto;
    }
    
    .spinner {
        position: relative;
        width: 45px;
        height: 45px;
        margin: 1em auto 0;
    }

    .bubble-1, .bubble-2 {
        position: absolute;
        top: 0;
        width: 25px;
        height: 25px;
        border-radius: 100%;
        background-color: #4b9cdb;
    }

    .bubble-2 {
        top: auto;
        bottom: 0;
    }
    
    .spinner {
        animation: loadingI 2s linear infinite;
    }

    .bubble-1, .bubble-2 {
        animation: bounce 2s ease-in-out infinite;
    }

    .bubble-2 {
        animation-delay: -1.0s;
    }

    @keyframes loadingI {
        100% {
            transform: rotate(360deg);
        }
    }

    @keyframes bounce {
        0%, 100% {
            transform: scale(0.0);
        }
        50% {
            transform: scale(1.0);
        }
    }
</style>
