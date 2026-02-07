export async function getSomething() {
  const res = await fetch("/health"); // or whatever endpoint you call

  // 204: no content
  if (res.status === 204) return { status: "No Content (204)" };

  const contentType = res.headers.get("content-type") ?? "";

  if (!res.ok) {
    const errText = await res.text().catch(() => "");
    throw new Error(`API failed (${res.status}): ${errText}`);
  }

  // If API returns JSON
  if (contentType.includes("application/json")) {
    return await res.json();
  }

  // Otherwise treat as text
  return await res.text();
}
