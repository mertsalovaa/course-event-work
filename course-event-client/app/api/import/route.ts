import { revalidatePath } from "next/cache";
import { triggerImport } from "../events";

export async function POST() {
  const result = await triggerImport();

  if (result.error) {
    return Response.json({ error: result.error.message }, { status: 500 });
  }

  revalidatePath("/");

  return Response.json(result.data);
}
