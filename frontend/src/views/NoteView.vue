<script setup>
import { ref, onMounted } from "vue";
import api from "../api.js";
import NoteModal from "../components/NoteModal.vue";

const items = ref([]);
const search = ref("");
const sortBy = ref("date-desc");
const loading = ref(true);

const showModal = ref(false);
const modalMode = ref("create");
const selectedNote = ref(null);

onMounted(loadNotes)

function openCreate() {
  modalMode.value = "create";
  selectedNote.value = { title: "", content: "" };
  showModal.value = true;
}

function openEdit(note) {
  modalMode.value = "edit";
  selectedNote.value = note;
  showModal.value = true;
}

function resetFilters() {
  search.value = "";
  sortBy.value = "date-desc"; // or default you want
  loadNotes();                // reload after reset
}

async function deleteNote(note) {
  await api.delete(`/note/${note.uid}`);

  await loadNotes();
}

async function loadNotes() {
  loading.value = true;
  const res = await api.get("/note/all", {
    params: {
      search: search.value,
      sortBy: sortBy.value,
    }
  });
  items.value = res.data.data;
  loading.value = false;
}

async function saveNote(note) {
  if (modalMode.value === "create") {

    await api.post("/note/create", {
      title: note.title,
      content: note.content
    });
  } else {

    await api.patch(`/note/${note.uid}`, {
      title: note.title,
      content: note.content
    });

  }
  // Reload notes
  await loadNotes();
}
function formatDate(dateStr) {
  if (!dateStr) return "None";

  const hasTimezone = /Z$|[+-]\d\d:\d\d$/.test(dateStr);
  const iso = hasTimezone ? dateStr : dateStr + "Z";

  const date = new Date(iso);
  return isNaN(date.getTime())
    ? "Invalid Date"
    : date.toLocaleString();
}
</script>

<template>

  <div class=" w-full lg:w-2/3 xl:w-1/2 mx-auto">

    <div class="py-8 flex flex-col gap-3 mb-6">
      <div class="join">
        <select class="select join-item mx-2" v-model="sortBy" @change="loadNotes">
          <option disabled selected>Sort</option>
          <option value="title-asc">Title ↑</option>
          <option value="title-desc">Title ↓</option>
          <option value="date-asc">Date ↑ Oldest</option>
          <option value="date-desc">Date ↓ Newest</option>
        </select>

        <div class="join-item mx-2">
          <input class="input" placeholder="Search By Title" v-model="search" @keyup.enter="loadNotes" />
        </div>
        <div class="join-item mx-2">
          <button class="btn btn-info text-white" @click="loadNotes">Search</button>
        </div>
                <button class="btn btn-warning text-white mx-2" @click="resetFilters">
          Reset
        </button>
        <button class="btn btn-success text-white" @click="openCreate">
          New Note
        </button>

      </div>
    </div>

    <div v-for="note in items" :key="note.uid">
      <div class="collapse collapse-arrow bg-base-100 border border-base-300">
        <input type="radio" name="my-accordion-2" checked="checked" />
        <div class="collapse-title font-semibold">{{ note.title }}</div>
        <div class="collapse-content text-sm">
          <p>{{ note.content }}</p>
          <p class="text-xs opacity-50 mt-2">
            Updated: {{ formatDate(note.updatedDate) }}
          </p>
          <p class="text-xs opacity-50 mt-2">
            Created: {{ formatDate(note.createdDate) }}
          </p>
          <button class="btn btn-success btn-sm mt-3 text-white" @click="openEdit(note)">
            Edit Note
          </button>
          <button class="btn btn-error btn-sm mt-3 ml-3 text-white" @click="deleteNote(note)">
            Delete Note
          </button>
        </div>
      </div>
    </div>
    <NoteModal v-model="showModal" :mode="modalMode" :note="selectedNote" @save="saveNote" />



  </div>
</template>
