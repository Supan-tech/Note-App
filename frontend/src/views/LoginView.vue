<script setup>
import { ref, onMounted } from "vue"
import { useRouter } from "vue-router"

const loginModal = ref(null)
const router = useRouter()

const loading = ref(false)
const errorMessage = ref("")

const form = ref({
    email: "",
    password: ""
})

onMounted(() => {
    loginModal.value.showModal()
})

function close() {
    loginModal.value.close()
    router.push("/")
}

async function submitLogin() {
    errorMessage.value = ""
    loading.value = true

    try {
        const response = await fetch("http://localhost:5001/api/auth/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                email: form.value.email,
                password: form.value.password
            })
        })

        const result = await response.json()

        if (!response.ok) {
            errorMessage.value =
                result.errors?.[0] || result.message || "Login failed"
            return
        }

        const { accessToken, user } = result.data

        localStorage.setItem("access_token", accessToken)
        localStorage.setItem("user", JSON.stringify(user))

        loginModal.value.close()
        router.push("/note")

    } catch (error) {
        errorMessage.value = "Cannot connect to server"
    } finally {
        loading.value = false
    }
}

function goToSignup() {
    loginModal.value.close()
    router.push("/signup")
}
</script>


<template>
    <dialog ref="loginModal" class="modal">
        <div class="modal-box">
            <h3 class="text-lg font-bold mb-4">Login</h3>

            <form @submit.prevent="submitLogin">
                <div class="form-control mb-3">
                    <label class="label">Email</label>
                    <input v-model="form.email" type="email" class="input input-bordered w-full" required />
                </div>

                <div class="form-control mb-3">
                    <label class="label">Password</label>
                    <input v-model="form.password" type="password" class="input input-bordered w-full" required />
                </div>

                <!-- Error message -->
                <div v-if="errorMessage" class="alert alert-error mb-3">
                    {{ errorMessage }}
                </div>
                <div class="text-center mt-2">
                    <span class="text-sm">Don't have an account?</span>
                    <a class="link link-info ml-1" @click.prevent="goToSignup">
                        Sign up
                    </a>
                </div>
                <div class="modal-action">
                    <button type="submit" class="btn btn-info text-white" :disabled="loading">
                        {{ loading ? "Logging in..." : "Login" }}
                    </button>

                    <button type="button" class="btn" @click="close">
                        Cancel
                    </button>
                </div>
            </form>
        </div>
    </dialog>
</template>
