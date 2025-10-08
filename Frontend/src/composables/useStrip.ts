export function useStrip(text: string, toStrip: string,){
    return text.replace(toStrip, '')
}