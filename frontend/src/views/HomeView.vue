<script setup>
import { onMounted } from "vue"
import { useRouter } from "vue-router"


const router = useRouter()

function isTokenValid(token) {
    if (!token) return false

    try {
        const payload = JSON.parse(atob(token.split(".")[1]))
        const now = Math.floor(Date.now() / 1000)

        return payload.exp && payload.exp > now
    } catch (e) {
        return false
    }
}

onMounted(() => {
    const token = localStorage.getItem("access_token")

    if (isTokenValid(token)) {
        router.replace("/note")  
    } else {
        localStorage.removeItem("access_token")
        localStorage.removeItem("user")
        router.replace("/login")  
    }
})
</script>

<template>
    <div class="flex items-center justify-center h-screen">
        <span class="loading loading-spinner loading-lg"></span>
    </div>
</template>
