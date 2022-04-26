export interface IHttpClient {
    get<T>(url: string): Promise<T>;
    getBlob(url: string): Promise<Blob>;
    post<T>(
        url: string,
        data?: any,
        appendHeaders?: Record<string, string>
    ): Promise<T>;

    uploadFilePost<T>(
        url: string,
        data?: any,
        appendHeaders?: Record<string, string>
    ): Promise<T>;

    put<T>(
        url: string,
        data?: any,
        appendHeaders?: Record<string, string>
    ): Promise<T>;

    delete<T>(
        url: string
    ): Promise<T>;
}