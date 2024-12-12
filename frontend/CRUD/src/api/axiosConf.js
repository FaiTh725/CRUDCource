import axios from "axios";

axios.defaults.headers.post['Content-Type'] = 'application/json';
axios.defaults.withCredentials = true;

const api = axios.create({
  baseURL: 'https://localhost:7004/api'
})

export default api;