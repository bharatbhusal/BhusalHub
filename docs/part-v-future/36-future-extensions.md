# 36 — Future Extensions

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal  
**References:** [35 - Roadmap](35-roadmap.md), [01 - Vision](../part-i-foundation/01-vision.md)

---

## Purpose

Document potential future capabilities that have been considered but are not yet committed to any specific release.

## Scope

Extensions beyond V4 that are speculative or contingent on external factors.

## Extension Catalog

### AI Search (CLIP)

Embedding-based semantic search for photos. Query by description ("beach sunset") rather than filename.

**Considerations:**
- Requires ONNX runtime or Python inference container.
- ~200–500 MB additional RAM.
- Embedding storage in SQLite (BLOB column) or separate vector store.

**Trigger:** User demand for search-by-content.

### Face Recognition

Detect faces in photos, group by person, enable person-based album views.

**Considerations:**
- Requires ML model (TensorFlow Lite or ONNX).
- Privacy concern: face data stays local.
- Processing time: ~1–2 seconds per photo on RPi.

**Trigger:** Photo library exceeds 5,000 photos.

### OCR Pipeline

Extract text from images and PDFs for full-text search.

**Considerations:**
- Tesseract OCR or similar.
- Text storage in SQLite FTS5 table.
- Processing overhead adds to upload pipeline.

**Trigger:** Document uploads become a primary use case.

### Home Automation Gateway

Integrate with Home Assistant to display dashboards, camera feeds, and sensor data.

**Considerations:**
- Home Assistant runs separately on the same Pi or another device.
- Integration via Home Assistant REST API or WebSocket.
- Requires additional RAM allocation (~200 MB for HA).

**Trigger:** Home automation becomes a priority.

### Git Hosting

Self-hosted Git repositories (Gitea or Forgejo).

**Considerations:**
- Separate Docker container.
- PostgreSQL or SQLite backend.
- Adds ~150 MB RAM.

**Trigger:** Need for private code hosting.

### Notes and Knowledge Management

Lightweight note-taking application integrated into the dashboard.

**Considerations:**
- Could use existing API with a new controller.
- Markdown-based storage.
- Simple enough for a single sprint.

**Trigger:** After V1 is stable.

### Mobile Applications

Native iOS and Android apps for media upload and browsing.

**Considerations:**
- React Native for code sharing.
- API already exists (REST).
- Background upload support needed.
- Push notifications (requires internet).

**Trigger:** Desktop browser usage proves the platform valuable.

### Multi-Node Clustering

Distribute services across multiple Raspberry Pi devices for increased capacity.

**Considerations:**
- Requires shared storage (NFS, MinIO).
- Requires shared session store (Redis).
- Load balancer (Nginx or HAProxy).
- Significant complexity increase.

**Trigger:** Single Pi is saturated and upgrade to larger hardware is not possible.

### Federation

Share albums or media with other BhusalHub instances or ActivityPub-compatible services.

**Considerations:**
- Protocol design (ActivityPub subset or custom).
- Access control for shared content.
- Internet dependency.

**Trigger:** Multiple instances exist and sharing is desired.

## Evaluation Criteria for Future Extensions

Every proposed extension MUST satisfy:

1. **Fits in the 2 GB RAM constraint** (or comes with a documented hardware upgrade).
2. **Can be containerized** (does not require OS-level changes).
3. **Does not break offline-first** (may require internet for some features, but core must work offline).
4. **Does not compromise privacy** (no external API calls for core functionality).
5. **Can be disabled** when not needed (resource-conscious).

## Related Chapters

- [35 - Roadmap](35-roadmap.md)
- [01 - Vision](../part-i-foundation/01-vision.md)
