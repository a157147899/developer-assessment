import apiClient from './apiClient';

interface Entity {
  id?: string;
}

class HttpService {
  endpoint: string;

  constructor(endpoint: string) {
    this.endpoint = endpoint;
  }

  getAll<T>() {
    const controller = new AbortController();
    const request = apiClient.get<T[]>(this.endpoint, { signal: controller.signal });
    return {
      request,
      cancel: () => {
        controller.abort();
      },
    };
  }

  create<T>(entity: T) {
    return apiClient.post(this.endpoint, entity);
  }

  update<T extends Entity>(entity: T) {
    return apiClient.put(this.endpoint + entity.id, entity);
  }

  delete(id: string) {
    return apiClient.delete(this.endpoint + id);
  }
}

const create = (endpoint: string) => new HttpService(endpoint);

export default create;
