export default function Container(props: { children: React.ReactNode }) {
  return (
    <main className="typeset typeset-site mx-auto max-w-7xl px-4 pt-8 sm:px-6 lg:px-8">
      {props.children}
    </main>
  )
}
