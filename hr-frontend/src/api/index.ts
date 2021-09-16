import axios from 'axios';

export default function getReport(url: string) {
  return axios.get(url)
    .then((response: any) => response.data)
    .catch((error: any) => {
      console.error(error);
    });
}
